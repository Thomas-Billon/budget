interface IButtonSwitchOption {
    value: ButtonSwitchValue;
    label?: string;
    icon?: string;
}

interface IButtonSwitchEvent {
    type: 'change' | 'other';
    value: ButtonSwitchValue;
}

const isButtonSwitchEvent = (obj: unknown): obj is IButtonSwitchEvent => {
    return typeof obj === 'object' && obj !== null && 'type' in obj && 'value' in obj;
}

type ButtonSwitchValue = string | number;

export { type IButtonSwitchOption, type IButtonSwitchEvent, type ButtonSwitchValue, isButtonSwitchEvent };
