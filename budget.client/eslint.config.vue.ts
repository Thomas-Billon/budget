const vueRules = {
    'no-useless-assignment': 'off',
    'no-use-before-define': 'off',
    'vue/require-default-prop': 'off',
    'vue/html-indent': ['error', 4],
    'vue/html-self-closing': ['error', {
        html: {
            void: 'always',
            normal: 'never',
            component: 'always'
        }
    }],
    'vue/max-attributes-per-line': ['error', {
        'singleline': { 'max': 99 },
        'multiline': { 'max': 1 }
    }],
    'vue/multiline-html-element-content-newline': ['error', { 'allowEmptyLines': true, 'ignoreWhenEmpty': true }],
    'vue/singleline-html-element-content-newline': 'off'
};

export default vueRules;
