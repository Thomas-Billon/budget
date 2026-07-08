import js from '@eslint/js';
import pluginVue from 'eslint-plugin-vue';
import tsPlugin from '@typescript-eslint/eslint-plugin';
import tsParser from '@typescript-eslint/parser';
import globals from 'globals';
import stylistic from '@stylistic/eslint-plugin';
import tsRules from './eslint.config.typescript';
import vueRules from './eslint.config.vue';
import stylisticRules from './eslint.config.stylistic';

export default [
    // Ignored paths
    {
        ignores: ['dist/**', 'coverage/**', 'index.html']
    },

    // JS recommended base for TS and Vue files
    {
        files: ['**/*.{ts,tsx,vue}'],
        ...js.configs.recommended
    },

    // Vue plugin setup + essential rules (scopes itself to **/*.vue internally)
    ...pluginVue.configs['flat/recommended'],

    // TypeScript files
    {
        files: ['**/*.{ts,tsx}'],
        plugins: { '@typescript-eslint': tsPlugin },
        languageOptions: {
            parser: tsParser,
            globals: {
                ...globals.browser,
                ...globals.es2021
            }
        },
        rules: tsRules
    },

    // Vue files
    {
        files: ['**/*.vue'],
        plugins: { '@typescript-eslint': tsPlugin },
        languageOptions: {
            globals: {
                ...globals.browser,
                ...globals.es2021
            },
            parserOptions: {
                parser: tsParser,
                extraFileExtensions: ['.vue']
            }
        },
        rules: {
            ...tsRules,
            ...vueRules
        }
    },

    // Stylistic rules for all relevant file types
    {
        files: ['**/*.{html,js,ts,jsx,tsx,vue,mdx}'],
        plugins: {
            '@stylistic': stylistic
        },
        rules: stylisticRules
    }
];
