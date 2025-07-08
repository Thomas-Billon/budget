import { globalIgnores } from 'eslint/config'
import { defineConfigWithVueTs, vueTsConfigs } from '@vue/eslint-config-typescript'
import pluginVue from 'eslint-plugin-vue'


export default defineConfigWithVueTs(
    {
        name: 'app/files-to-lint',
        files: ['**/*.{html,js,ts,jsx,tsx,vue,mdx}'],
    },

    globalIgnores(['**/dist/**', '**/dist-ssr/**', '**/coverage/**']),

    pluginVue.configs['flat/strongly-recommended'],
    vueTsConfigs.recommended,

    {
        rules: {
            // "vue/require-default-prop": "on",
        }
    }
)
