import { request } from './http'

export const registerUser = (payload) =>
  request('/api/v1/auth/register', {
    method: 'POST',
    body: JSON.stringify(payload),
  })

export const loginUser = async (payload) => {
  const data = await request('/api/v1/auth/login', {
    method: 'POST',
    body: JSON.stringify(payload),
  })

  localStorage.setItem('jilo_is_authenticated', 'true')

  return data
}

export const logoutUser = () =>
  request('/api/v1/auth/logout', {
    method: 'POST',
  })

export const refreshSession = () =>
  request('/api/v1/auth/refresh', {
    method: 'POST',
  })
