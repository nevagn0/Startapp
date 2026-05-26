import { handleUnauthorized } from './authSession'

const PROBLEM_JSON = 'application/problem+json'
const AUTH_ENDPOINTS = [
  '/api/v1/auth/login',
  '/api/v1/auth/register',
  '/api/v1/auth/refresh',
  '/api/v1/auth/logout',
]

let refreshPromise = null

const getContentType = (headers) => headers.get('content-type') || ''

const parseBody = async (response) => {
  const contentType = getContentType(response.headers)
  if (contentType.includes('application/json') || contentType.includes(PROBLEM_JSON)) {
    return response.json()
  }

  if (contentType.includes('text/plain') || contentType.includes('text/json')) {
    return response.text()
  }

  return null
}

const shouldAttemptRefresh = (url) => !AUTH_ENDPOINTS.some((endpoint) => url.startsWith(endpoint))

const tryRefreshSession = async () => {
  if (!refreshPromise) {
    refreshPromise = fetch('/api/v1/auth/refresh', {
      method: 'POST',
      credentials: 'include',
    })
      .then((response) => ({
        ok: response.ok,
        status: response.status,
      }))
      .catch(() => ({
        ok: false,
        status: 0,
      }))
      .finally(() => {
        refreshPromise = null
      })
  }

  return refreshPromise
}

const buildError = (body, status) => {
  const problemDetail =
    typeof body === 'object' && body !== null
      ? body.detail || body.title
      : null

  const error = new Error(problemDetail || `Ошибка запроса (${status})`)
  error.status = status
  return error
}

export const request = async (url, options = {}) => {
  const isFormData = options.body instanceof FormData
  const headers = {
    ...(!isFormData ? { 'Content-Type': 'application/json' } : {}),
    ...options.headers,
  }

  let response = await fetch(url, {
    credentials: 'include',
    headers,
    ...options,
  })

  if (response.status === 401 && shouldAttemptRefresh(url)) {
    const refreshResult = await tryRefreshSession()

    if (refreshResult.ok) {
      response = await fetch(url, {
        credentials: 'include',
        headers,
        ...options,
      })
    } else if (refreshResult.status === 401) {
      handleUnauthorized()
    }
  }

  const body = await parseBody(response)

  if (!response.ok) {
    if (response.status === 401) {
      handleUnauthorized()
    }
    throw buildError(body, response.status)
  }

  return body
}
