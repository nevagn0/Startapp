import { request } from './http'

export const searchPlayers = ({ gameId, role, rank }) => {
  const params = new URLSearchParams()
  params.set('gameId', gameId)

  if (role) params.set('role', role)
  if (rank) params.set('rank', rank)

  return request(`/api/v1/search/players?${params.toString()}`)
}
