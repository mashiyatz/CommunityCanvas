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
  base: "./",
  build: {
    sourcemap: true,
    commonjsOptions: {
      // include: ["/node_modules"],
      extensions: ['.js', '.cjs'],
      strictRequires: true,
      // // https://stackoverflow.com/questions/62770883/how-to-include-both-import-and-require-statements-in-the-bundle-using-rollup
      transformMixedEsModules: true,
    },
    rollupOptions: {
      // external: [/node_modules/],
    },
    manifest: true,
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
      // https://github.com/aws-amplify/amplify-js/issues/9639    
      './runtimeConfig': './runtimeConfig.browser',
    },
  }
})
