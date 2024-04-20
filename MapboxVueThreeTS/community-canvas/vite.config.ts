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
  base: "./dist/",
  build: {
    sourcemap: true,
    // commonjsOptions: {
    //   include: ["/node_modules"]
    // },
    rollupOptions: {
      external: [/node_modules/],
    },
    manifest: true,
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})
