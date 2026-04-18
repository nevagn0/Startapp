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

  if (data && typeof data === 'object' && data.accessToken) {
    localStorage.setItem('jilo_access_token', data.accessToken)
  }

  return data
}

export const logoutUser = () =>
  request('/api/v1/auth/logout', {
    method: 'POST',
  })
