const stylisticRules = {
    '@stylistic/semi': ['error', 'always'],
    '@stylistic/quotes': ['error', 'single', { 'avoidEscape': true }],
    '@stylistic/no-multi-spaces': ['error'],
    '@stylistic/no-multiple-empty-lines': ['error', { 'max': 2 }],
    '@stylistic/space-before-blocks': ['error', 'always'],
    '@stylistic/eol-last': ['error', 'always'],
    '@stylistic/dot-location': ['error', 'property'],
    '@stylistic/key-spacing': ['error', { 'mode': 'strict' }],
    '@stylistic/keyword-spacing': ['error', { 'before': true, 'after': true }],
    '@stylistic/max-statements-per-line': ['error', { 'max': 1 }],
    '@stylistic/new-parens': ['error', 'always'],
    '@stylistic/brace-style': ['error', 'stroustrup', { 'allowSingleLine': true }],
    '@stylistic/comma-style': ['error', 'last'],
    '@stylistic/comma-dangle': ['error', 'never'],
    '@stylistic/comma-spacing': ['error', { 'before': false, 'after': true }],
    '@stylistic/arrow-spacing': ['error', { 'before': true, 'after': true }],
    '@stylistic/block-spacing': ['error', 'always'],
    '@stylistic/array-bracket-spacing': ['error', 'never'],
    '@stylistic/object-curly-spacing': ['error', 'always', { 'objectsInObjects': false }],
    '@stylistic/object-curly-newline': ['error', { 'consistent': true, 'multiline': true }],
    '@stylistic/computed-property-spacing': ['error', 'never']
};

export default stylisticRules;
