let unauthorizedHandler = null

export const setUnauthorizedHandler = (handler) => {
  unauthorizedHandler = handler
}

export const handleUnauthorized = () => {
  if (typeof unauthorizedHandler === 'function') {
    unauthorizedHandler()
  }
}
