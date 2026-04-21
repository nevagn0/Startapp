import { request } from './http'

export const createLobby = (gameId) =>
  request('/api/v1/lobby', {
    method: 'POST',
    body: JSON.stringify({ gameId }),
  })

export const getCurrentLobby = () => request('/api/v1/lobby/current')

export const getLobbyById = (lobbyId) => request(`/api/v1/lobby/${lobbyId}`)

export const leaveLobby = (lobbyId) =>
  request(`/api/v1/lobby/${lobbyId}/leave`, {
    method: 'POST',
  })

export const deleteLobby = (lobbyId) =>
  request(`/api/v1/lobby/${lobbyId}`, {
    method: 'DELETE',
  })

export const kickFromLobby = (lobbyId, profileId) =>
  request(`/api/v1/lobby/${lobbyId}/kick/${profileId}`, {
    method: 'POST',
  })

export const sendLobbyInvitation = (lobbyId, receiverProfileId) =>
  request(`/api/v1/lobby/${lobbyId}/invitations`, {
    method: 'POST',
    body: JSON.stringify({ recieverProfileId: receiverProfileId }),
  })

export const getOutgoingLobbyInvitations = (lobbyId) =>
  request(`/api/v1/lobby/${lobbyId}/invitations`)
