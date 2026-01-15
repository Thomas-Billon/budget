<script setup lang="ts">

    import './ButtonSwitch.scss';

    import { onMounted, ref, watch } from 'vue';
    import type { IButtonSwitchOption, ButtonSwitchValue } from '@/components/button-switch/ButtonSwitch';

    interface Props {
        options: IButtonSwitchOption[];
        className?: string;
    };

    type Emits = {
        change: [value: ButtonSwitchValue];
    };

    const { options, className } = defineProps<Props>();
    const model = defineModel<ButtonSwitchValue | undefined>({ required: true });
    const emit = defineEmits<Emits>();
    
    const switchIndex = ref<number>(-1);
    const switchCount = ref<number>(0);
    
    // Init
    onMounted(() => {
        updateSwitchIndex();
        updateSwitchCount();
    });

    // On props change
    watch(() => options, (_) => {
        updateSwitchCount();
    });

    // On model change
    watch(() => model.value, (_) => {
        updateSwitchIndex();
    });

    const updateSwitchIndex = (): void => {
        switchIndex.value = options.findIndex(option => option.value === model.value);
    };

    const updateSwitchCount = (): void => {
        switchCount.value = options?.length ?? 0;
    };

    const onSwitch = (value: ButtonSwitchValue): void => {
        model.value = value;
        emit('change', value);
    };

</script>

<template>
    <div v-if="switchCount > 0" :class="[ 'button-switch', className, switchIndex === -1 ? 'no-active' : 'has-active' ]" :style="{ '--switchIndex': switchIndex, '--switchCount': switchCount }">
        <div class="button-switch-container">
            <button type="button" v-for="option in options" :key="option.value" class="button-switch-option btn btn-white" @click="onSwitch(option.value)">
                <font-awesome-icon v-if="option.icon" :icon="`fa-solid fa-${option.icon}`" size="sm" />
                <span v-if="option.label" >{{ option.label }}</span>
            </button>
            <div class="button-switch-highlight btn btn-white"></div>
        </div>
    </div>
</template>
