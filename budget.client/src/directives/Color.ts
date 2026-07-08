import type { ObjectDirective, DirectiveBinding } from 'vue';

const hexToRgb = (hex: string): string => {
    let hexValue = hex.replace('#', '');

    // FFF -> FFFFFF
    if (hexValue.length === 3) {
        hexValue = hexValue.split('').map(c => c + c).join('');
    }

    const num = parseInt(hexValue, 16);
    const r = (num >> 16) & 255;
    const g = (num >> 8) & 255;
    const b = num & 255;

    return [r, g, b].join(',');
};

const setStyle = (el: HTMLElement, hex?: string) => {
    if (hex === undefined) {
        return;
    }

    if (typeof hex !== 'string') {
        console.error('Error: v-color directive requires a string value.');
        return;
    }

    el.style.setProperty('--color', hex);
    el.style.setProperty('--color-rgb', hexToRgb(hex));
};

const vColor: ObjectDirective<HTMLElement, string | undefined> = {
    mounted(el: HTMLElement, binding: DirectiveBinding<string | undefined>) {
        setStyle(el, binding.value);
    },
    updated(el: HTMLElement, binding: DirectiveBinding<string | undefined>) {
        if (binding.value === binding.oldValue) {
            return;
        }

        setStyle(el, binding.value);
    }
};

export default vColor;
