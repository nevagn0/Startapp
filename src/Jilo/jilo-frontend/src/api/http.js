const PROBLEM_JSON = 'application/problem+json'

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

export const request = async (url, options = {}) => {
  const response = await fetch(url, {
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
      ...options.headers,
    },
    ...options,
  })

  const body = await parseBody(response)

  if (!response.ok) {
    const problemDetail =
      typeof body === 'object' && body !== null
        ? body.detail || body.title
        : null

    throw new Error(problemDetail || `Ошибка запроса (${response.status})`)
  }

  return body
}
