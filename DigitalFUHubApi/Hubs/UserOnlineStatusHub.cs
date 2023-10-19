﻿using BusinessObject.Entities;
using Comons;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using DigitalFUHubApi.Managers;
using DigitalFUHubApi.Services;
using DTOs.Conversation;
using DTOs.Notification;
using DTOs.User;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace DigitalFUHubApi.Hubs
{
	public class UserOnlineStatusHub : Hub
	{
		private readonly IConnectionManager connectionManager;
		private readonly IConversationRepository conversationRepository;
		private readonly IUserRepository userRepository;
		private readonly HubService hubService;

		public UserOnlineStatusHub(IConnectionManager connectionManager, IConversationRepository conversationRepository, IUserRepository userRepository, HubService hubService)
		{
			this.connectionManager = connectionManager;
			this.conversationRepository = conversationRepository;
			this.userRepository = userRepository;
			this.hubService = hubService;
		}

		public override async Task OnConnectedAsync()
		{

			var userId = hubService.GetUserIdFromHubCaller(Context);
			// check user has been open in orther divice
			var isUserConnectd = connectionManager.CheckUserConnected(userId, Constants.SIGNAL_R_USER_ONLINE_STATUS_HUB);
			if (isUserConnectd) return;

			// add new connection
			var currentConnectionId = hubService.GetConnectionIdFromHubCaller(Context);
			connectionManager.AddConnection(userId, currentConnectionId, Constants.SIGNAL_R_USER_ONLINE_STATUS_HUB);

			// get all user has conversation with current user
			List<UserConversationDTO> recipients = conversationRepository.GetRecipientUserIdHasConversation(userId);

			// send status online to all recipients online
			foreach (var recipient in recipients)
			{
				var connectionIds = connectionManager.GetConnections(recipient.UserId, Constants.SIGNAL_R_USER_ONLINE_STATUS_HUB);
				if (connectionIds == null || connectionIds.Count == 0) continue;
				foreach (var connectionId in connectionIds)
				{
				 	await SendUserOnlineStatus(recipient.ConversationId, true, connectionId);
				}
			}

			//Update DB User
			userRepository.UpdateUserOnlineStatus(userId, true);
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var userId = hubService.GetUserIdFromHubCaller(Context);
			var currentConnectionId = hubService.GetConnectionIdFromHubCaller(Context);
			//remove connection
			connectionManager.RemoveConnection(userId, currentConnectionId , Constants.SIGNAL_R_USER_ONLINE_STATUS_HUB_RECEIVE_ONLINE_STATUS);

			// check user has been open in orther divice
			var isUserConnectd = connectionManager.CheckUserConnected(userId, Constants.SIGNAL_R_USER_ONLINE_STATUS_HUB);
			if (isUserConnectd) return;

			// get all user has conversation with current user
			List<UserConversationDTO> recipients = conversationRepository.GetRecipientUserIdHasConversation(userId);
			// send status online to all recipients online
			foreach (var recipient in recipients)
			{
				var connectionIds = connectionManager.GetConnections(userId, Constants.SIGNAL_R_USER_ONLINE_STATUS_HUB);
				if (connectionIds == null || connectionIds.Count == 0) continue;
				if (recipient.IsGroup)
				{
					int numberMemeberInGroupOnline = 0;
					// count number user remaning existed online
					foreach (var member in recipient.MembersInGroup)
					{
						var isMemberOnline = connectionManager.CheckUserConnected(userId, Constants.SIGNAL_R_USER_ONLINE_STATUS_HUB);
						if(isMemberOnline) numberMemeberInGroupOnline++;	
					}
					if(numberMemeberInGroupOnline == 0) 
					{
						foreach (var connectionId in connectionIds)
						{
							await SendUserOnlineStatus(recipient.ConversationId, false, connectionId);
						}
					}
				}
				else
				{
					foreach (var connectionId in connectionIds)
					{
						await SendUserOnlineStatus(recipient.ConversationId, false, connectionId);
					}
				}
				
			}

			//Update DB User
			userRepository.UpdateUserOnlineStatus(userId, false);

			return;
		}

		private async Task SendUserOnlineStatus(long conversationId, bool isOnline, string connectionId)
		{
			await Clients.Clients(connectionId)
				.SendAsync(Constants.SIGNAL_R_USER_ONLINE_STATUS_HUB_RECEIVE_ONLINE_STATUS,
					JsonConvert.SerializeObject(
						new UserOnlineStatusHubDTO { 
							ConversationId = conversationId,
							IsOnline = isOnline,	
						})
				);
		}

	}
}
