interface IButtonSwitchOption {
    value: ButtonSwitchValue;
    label?: string;
    icon?: string;
}

type ButtonSwitchValue = string | number;

export { type IButtonSwitchOption, type ButtonSwitchValue };
