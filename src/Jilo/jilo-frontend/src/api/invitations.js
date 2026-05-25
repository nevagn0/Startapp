import { request } from './http'

export const getIncomingInvitations = () => request('/api/v1/invitations/incoming')

export const acceptInvitation = (invitationId) =>
  request(`/api/v1/invitations/${invitationId}/accept`, {
    method: 'POST',
  })

export const declineInvitation = (invitationId) =>
  request(`/api/v1/invitations/${invitationId}/decline`, {
    method: 'POST',
  })

export const cancelInvitation = (invitationId) =>
  request(`/api/v1/invitations/${invitationId}`, {
    method: 'DELETE',
  })
