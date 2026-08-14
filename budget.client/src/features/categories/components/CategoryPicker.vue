<script setup lang="ts">

    import './CategoryPicker.scss';

    import { type ICategoryOptionsItemResponse } from '@/features/categories/models/ICategoryOptionsResponse';
    import { getCategoryColorHex, getCategoryIconClass } from '@/features/categories/CategoryService';
    import useSelection from '@/composables/useSelection';

    interface Props {
        options: ICategoryOptionsItemResponse[];
    };

    type Emits = {
        change: [value: number[]];
    };

    const { options } = defineProps<Props>();
    const model = defineModel<number[]>({ required: true });
    const emit = defineEmits<Emits>();

    const { isSelected, select } = useSelection(model, emit, { isMultiple: true });

</script>

<template>
    <div v-if="options.length > 0" class="category-picker">
        <button
            v-for="option in options"
            :key="option.id"
            v-color="getCategoryColorHex(option.color)"
            type="button"
            :title="option.name"
            :class="['category-picker-option', { selected: isSelected(option.id) }]"
            @click="select(option.id)"
        >
            <span class="category-picker-option-icon">
                <font-awesome-icon :icon="`fa-solid fa-${getCategoryIconClass(option.icon)}`" />
            </span>
            <span class="category-picker-option-name">{{ option.name }}</span>
        </button>
    </div>
    <p v-else class="category-picker-empty">No categories yet.</p>
</template>
