using AutoMapper;
using CMS.Data.Entities.Systems;
using CMS.Repositories;
using CMS.ViewModels.Catalog;
using CMS.ViewModels.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace CMS.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public readonly static List<UserViewModel> _connections = new List<UserViewModel>();
        private readonly IUserRepository _userRepository;
        private readonly static Dictionary<string, string> _connectionsMap = new Dictionary<string, string>();
        private readonly IMapper _mapper;

        public ChatHub(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task SendPrivate(string receiverName, string message)
        {
            if (_connectionsMap.TryGetValue(receiverName, out string? userId))
            {
                // Who is the sender;
                var sender = _connections.Where(u => u.UserName == IdentityName).First();

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    // Build the message
                    var messageViewModel = new MessageViewModel()
                    {
                        Content = Regex.Replace(message, @"<.*?>", string.Empty),
                        FromUserName = sender.UserName,
                        FromFullName = sender.FullName,
                        Avatar = sender.Avatar,
                        Room = "",
                        Timestamp = DateTime.Now
                    };

                    // Send the message
                    await Clients.Client(userId!).SendAsync("newMessage", messageViewModel);
                    await Clients.Caller.SendAsync("newMessage", messageViewModel);
                }
            }
        }

        public async Task Join(string roomName)
        {
            try
            {
                var user = _connections.Where(u => u.UserName == IdentityName).FirstOrDefault();
                if (user != null && user.CurrentRoom != roomName)
                {
                    // Remove user from others list
                    if (!string.IsNullOrEmpty(user.CurrentRoom))
                        await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                    // Join to new chat room
                    await Leave(user.CurrentRoom!);
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    user.CurrentRoom = roomName;

                    // Tell others to update their list of users
                    await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public IEnumerable<UserViewModel> GetUsers(string roomName)
        {
            return _connections.Where(u => u.CurrentRoom == roomName).ToList();
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = await _userRepository.GetUserByNameAsync(IdentityName);
                if (user == null)
                {
                    await Clients.Caller.SendAsync("onError", "User not found!");
                    return;
                }

                var userViewModel = _mapper.Map<User, UserViewModel>(user);
                userViewModel.Device = GetDevice();
                userViewModel.CurrentRoom = "";

                if (!_connections.Any(u => u.UserName == IdentityName))
                {
                    _connections.Add(userViewModel);
                    _connectionsMap.Add(IdentityName, Context.ConnectionId);
                }

                await Clients.Caller.SendAsync("getProfileInfo", userViewModel);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var user = _connections.Where(u => u.UserName == IdentityName).First();
                _connections.Remove(user);

                // Tell other users to remove you from their list
                await Clients.OthersInGroup(user.CurrentRoom!).SendAsync("removeUser", user);

                // Remove mapping
                _connectionsMap.Remove(user.UserName!);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private string IdentityName
        {
            get { return Context.User!.Identity!.Name!; }
        }

        private string GetDevice()
        {
            var device = Context.GetHttpContext()!.Request.Headers["Device"].ToString();
            if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
                return device;

            return "Web";
        }
    }
}
