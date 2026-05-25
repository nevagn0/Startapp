import { useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { AuthContext } from './auth-context'
import { logoutUser } from '../api/auth'
import { setUnauthorizedHandler } from '../api/authSession'

const INACTIVITY_TIMEOUT_MS = 5 * 60 * 1000

export function AuthProvider({ children }) {
  const timerRef = useRef(null)
  const [isAuthenticated, setIsAuthenticated] = useState(
    localStorage.getItem('jilo_is_authenticated') === 'true',
  )

  const clearAuthState = useCallback(() => {
    localStorage.removeItem('jilo_is_authenticated')
    setIsAuthenticated(false)
  }, [])

  const resetInactivityTimer = useCallback(() => {
    if (timerRef.current) {
      window.clearTimeout(timerRef.current)
    }

    if (!isAuthenticated) {
      return
    }

    timerRef.current = window.setTimeout(() => {
      clearAuthState()
      window.location.assign('/login')
    }, INACTIVITY_TIMEOUT_MS)
  }, [clearAuthState, isAuthenticated])

  const signIn = useCallback(() => {
    localStorage.setItem('jilo_is_authenticated', 'true')
    setIsAuthenticated(true)
  }, [])

  const signOut = useCallback(async () => {
    try {
      await logoutUser()
    } finally {
      clearAuthState()
    }
  }, [clearAuthState])

  useEffect(() => {
    setUnauthorizedHandler(() => {
      clearAuthState()
      window.location.assign('/login')
    })

    return () => setUnauthorizedHandler(null)
  }, [clearAuthState])

  useEffect(() => {
    if (!isAuthenticated) {
      if (timerRef.current) {
        window.clearTimeout(timerRef.current)
      }
      return
    }

    const events = ['mousemove', 'keydown', 'scroll', 'click', 'touchstart']
    events.forEach((eventName) => {
      window.addEventListener(eventName, resetInactivityTimer)
    })

    resetInactivityTimer()

    return () => {
      events.forEach((eventName) => {
        window.removeEventListener(eventName, resetInactivityTimer)
      })
      if (timerRef.current) {
        window.clearTimeout(timerRef.current)
      }
    }
  }, [isAuthenticated, resetInactivityTimer])

  const value = useMemo(
    () => ({
      isAuthenticated,
      signIn,
      signOut,
      resetInactivityTimer,
    }),
    [isAuthenticated, signIn, signOut, resetInactivityTimer],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
