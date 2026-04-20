import { request } from './http'

export const getMyProfile = () => request('/api/v1/profile/me')

export const updateMyBio = (bio) =>
  request('/api/v1/profile/me', {
    method: 'PATCH',
    body: JSON.stringify({ bio }),
  })

export const uploadMyAvatar = (file) => {
  const formData = new FormData()
  formData.append('file', file)

  return request('/api/v1/profile/me/avatar', {
    method: 'POST',
    body: formData,
  })
}

export const getMyGames = () => request('/api/v1/profile/me/games')

export const addMyGame = (payload) =>
  request('/api/v1/profile/me/games', {
    method: 'POST',
    body: JSON.stringify(payload),
  })

export const updateMyGame = (userGameId, payload) =>
  request(`/api/v1/profile/me/games/${userGameId}`, {
    method: 'PATCH',
    body: JSON.stringify(payload),
  })

export const deleteMyGame = (userGameId) =>
  request(`/api/v1/profile/me/games/${userGameId}`, {
    method: 'DELETE',
  })

export const getAvailableGames = () => request('/api/v1/games')
