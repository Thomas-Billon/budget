<script setup lang="ts">

    import './ColorPicker.scss';

    import useSelection from '@/composables/useSelection';

    interface Props {
        options: string[];
    };

    type Emits = {
        change: [value: string];
    };

    const { options } = defineProps<Props>();
    const model = defineModel<string | undefined>({ required: true });
    const emit = defineEmits<Emits>();

    const { isSelected, select } = useSelection(model, emit, { isMultiple: false });

</script>

<template>
    <div v-if="options.length > 0" class="color-picker">
        <button
            v-for="(option, index) in options"
            :key="index"
            v-color="option"
            type="button"
            :class="['color-picker-option', { selected: isSelected(option) }]"
            @click="select(option)"
        >
            <font-awesome-icon v-if="isSelected(option)" icon="fa-solid fa-check" />
        </button>
    </div>
    <p v-else class="color-picker-empty">No colors available.</p>
</template>
