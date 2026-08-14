<script setup lang="ts">

    import './ButtonSwitch.scss';

    import { nextTick, onBeforeUpdate, onMounted, onUnmounted, ref, watch } from 'vue';
    import type { IButtonSwitchOption, ButtonSwitchValue } from '@/components/button-switch/ButtonSwitch';
    import useSelection from '@/composables/useSelection';

    interface Props {
        options: IButtonSwitchOption[];
    };

    type Emits = {
        change: [value: ButtonSwitchValue];
    };

    const { options } = defineProps<Props>();
    const model = defineModel<ButtonSwitchValue | undefined>({ required: true });
    const emit = defineEmits<Emits>();

    const { isSelected, select } = useSelection(model, emit, { isMultiple: false });

    const switchIndex = ref<number>(-1);
    const switchCount = ref<number>(0);
    const switchRef = ref<HTMLElement>();
    const scrollRef = ref<HTMLElement>();
    const optionRefs = ref<HTMLElement[]>([]);
    const highlightStyle = ref<{ '--optionPosition'?: string; '--optionWidth'?: string }>({});
    const isHighlighted = ref<boolean>(false);
    const isDragging = ref<boolean>(false);

    const DRAG_THRESHOLD_PX = 5;

    let dragStartX: number | null = null;
    let dragStartScrollLeft = 0;

    let resizeObserver: ResizeObserver | undefined;

    onBeforeUpdate(() => {
        optionRefs.value = [];
    });

    // Init
    onMounted(async () => {
        updateSwitchIndex();
        updateSwitchCount();
        await nextTick();
        updateHighlightStyle();
        scrollActiveOptionIntoView({ isSmoothScroll: false });

        if (switchRef.value) {
            resizeObserver = new ResizeObserver((_) => {
                updateHighlightStyle();
                scrollActiveOptionIntoView({ isSmoothScroll: false });
            });
            resizeObserver.observe(switchRef.value);
        }
    });

    onUnmounted(() => {
        resizeObserver?.disconnect();
        window.removeEventListener('mousemove', onMouseMove);
        window.removeEventListener('mouseup', onMouseUp);
    });

    // On props change
    watch(() => options, async (_) => {
        updateSwitchCount();
        await nextTick();
        updateHighlightStyle();
        scrollActiveOptionIntoView({ isSmoothScroll: true });
    });

    // On model change
    watch(() => model.value, async (_) => {
        updateSwitchIndex();
        await nextTick();
        updateHighlightStyle();
        scrollActiveOptionIntoView({ isSmoothScroll: true });
    });

    const updateSwitchIndex = (): void => {
        switchIndex.value = options.findIndex(option => option.value === model.value);
    };

    const updateSwitchCount = (): void => {
        switchCount.value = options?.length ?? 0;
    };

    const updateHighlightStyle = (): void => {
        const activeOption = optionRefs.value[switchIndex.value];
        if (!activeOption) {
            return;
        }

        highlightStyle.value = {
            '--optionPosition': `${activeOption.offsetLeft}px`,
            '--optionWidth': `${activeOption.offsetWidth}px`
        };
        isHighlighted.value = true;
    };

    const scrollActiveOptionIntoView = ({ isSmoothScroll }: { isSmoothScroll?: boolean } = { isSmoothScroll: true }): void => {
        const activeOption = optionRefs.value[switchIndex.value];
        activeOption?.scrollIntoView({ behavior: isSmoothScroll ? 'smooth' : 'auto', inline: 'center', block: 'center' });
    };

    const onMouseDown = (event: MouseEvent): void => {
        if (!scrollRef.value) {
            return;
        }

        dragStartX = event.clientX;
        dragStartScrollLeft = scrollRef.value.scrollLeft;
        isDragging.value = false;

        window.addEventListener('mousemove', onMouseMove);
        window.addEventListener('mouseup', onMouseUp);
    };

    const onMouseMove = (event: MouseEvent): void => {
        if (dragStartX === null || !scrollRef.value) {
            return;
        }

        const delta = event.clientX - dragStartX;

        if (!isDragging.value && Math.abs(delta) > DRAG_THRESHOLD_PX) {
            isDragging.value = true;
        }

        if (isDragging.value) {
            scrollRef.value.scrollLeft = dragStartScrollLeft - delta;
        }
    };

    const onMouseUp = (): void => {
        window.removeEventListener('mousemove', onMouseMove);
        window.removeEventListener('mouseup', onMouseUp);

        isDragging.value = false;
        dragStartX = null;
    };

</script>

<template>
    <div
        v-if="switchCount > 0"
        ref="switchRef"
        class="button-switch"
        :class="switchIndex !== -1 ? 'has-active' : 'no-active'"
    >
        <div
            ref="scrollRef"
            class="button-switch-scroll"
            :class="{ 'is-dragging': isDragging }"
            @mousedown="onMouseDown"
        >
            <div class="button-switch-content">
                <div class="button-switch-options">
                    <button
                        v-for="(option, index) in options"
                        ref="optionRefs"
                        :key="index"
                        type="button"
                        class="button-switch-option btn btn-link"
                        :class="{ 'active': isSelected(option.value) }"
                        @click="select(option.value)"
                    >
                        <font-awesome-icon v-if="option.icon" :icon="`fa-solid fa-${option.icon}`" />
                        <span v-if="option.label">{{ option.label }}</span>
                    </button>
                    <div class="button-switch-highlight btn btn-white" :class="{ 'ready': isHighlighted }" :style="highlightStyle"></div>
                </div>
            </div>
        </div>
    </div>
</template>
