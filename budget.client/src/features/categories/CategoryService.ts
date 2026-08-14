import { CategoryColor } from '@/enums/CategoryColor';
import { CategoryIcon } from '@/enums/CategoryIcon';

// Keep in sync with $category-colors in variables.scss.
const categoryColorHexMap: Record<CategoryColor, string> = {
    [CategoryColor.None]: '#707070',
    [CategoryColor.Blue]: '#3D6FA6',
    [CategoryColor.Green]: '#2E8F63',
    [CategoryColor.Yellow]: '#B9862A',
    [CategoryColor.Orange]: '#B9663B',
    [CategoryColor.Red]: '#BA3A3A'
};

export const getCategoryColorHex = (color: CategoryColor): string => categoryColorHexMap[color] ?? categoryColorHexMap[CategoryColor.None];

export const getCategoryColor = (colorHex: string): CategoryColor => {
    const color = Object.entries(categoryColorHexMap).find(([_key, value]) => value === colorHex)?.[0];
    return color !== undefined ? Number(color) as CategoryColor : CategoryColor.None;
};

const categoryIconClassMap: Record<CategoryIcon, string> = {
    [CategoryIcon.None]: 'receipt',
    [CategoryIcon.Bank]: 'landmark',
    [CategoryIcon.Bill]: 'file-invoice',
    [CategoryIcon.House]: 'house',
    [CategoryIcon.Wallet]: 'wallet',
    [CategoryIcon.CreditCard]: 'credit-card',
    [CategoryIcon.Coins]: 'coins',
    [CategoryIcon.PiggyBank]: 'piggy-bank',
    [CategoryIcon.Cart]: 'cart-shopping',
    [CategoryIcon.Gift]: 'gift',
    [CategoryIcon.Car]: 'car',
    [CategoryIcon.Plane]: 'plane',
    [CategoryIcon.Restaurant]: 'utensils'
};

export const getCategoryIconClass = (icon: CategoryIcon): string => categoryIconClassMap[icon] ?? categoryIconClassMap[CategoryIcon.None];

export const getCategoryIcon = (iconClass: string): CategoryIcon => {
    const icon = Object.entries(categoryIconClassMap).find(([_key, value]) => value === iconClass)?.[0];
    return icon !== undefined ? Number(icon) as CategoryIcon : CategoryIcon.None;
};
