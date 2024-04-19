import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue({
      template: {
        compilerOptions: {
          isCustomElement: (tag) => tag.startsWith("SideMenu.vue"),
        }
      }
    })
  ],
  base: "/CommunityCanvas/",
  build: {
     rollupOptions: {
      external: ['mapbox-gl'],
      output: {
        globals: {
          mapbox: 'mapbox-gl',

        }
      }
    }
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})
