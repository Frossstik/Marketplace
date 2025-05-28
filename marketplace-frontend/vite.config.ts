import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    allowedHosts: [
      'localhost',
      'a84d-103-231-73-211.ngrok-free.app' 
    ],
    cors: true 
  }
  
})
