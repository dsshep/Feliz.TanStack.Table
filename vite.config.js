import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
    root: "src/Docs",
    build: {
        outDir: "../../.fable-build",
        emptyOutDir: true,
        sourcemap: true
    },
    plugins: [
        react()
    ]
});