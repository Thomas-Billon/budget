<script setup lang="ts">

    import './IconPicker.scss';

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
    <div v-if="options.length > 0" class="icon-picker">
        <button
            v-for="(option, index) in options"
            :key="index"
            type="button"
            :class="['icon-picker-option', { selected: isSelected(option) }]"
            @click="select(option)"
        >
            <font-awesome-icon :icon="`fa-solid fa-${option}`" />
        </button>
    </div>
    <p v-else class="icon-picker-empty">No icons available.</p>
</template>
