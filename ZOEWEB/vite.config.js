// vite.config.js (Vue)
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'

export default defineConfig({
  plugins: [vue()],
  build: {
    outDir: '../API/ClientApp/dist',
    emptyOutDir: true
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
      '@mod1': path.resolve(__dirname, './ZoeMod01/src'),
      
     
    }
  }
})