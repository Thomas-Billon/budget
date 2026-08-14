<script setup lang="ts">

    import './CategoryForm.scss';

    import { computed } from 'vue';
    import { CategoryColor } from '@/enums/CategoryColor.ts';
    import { CategoryIcon } from '@/enums/CategoryIcon.ts';
    import { getCategoryColor, getCategoryColorHex, getCategoryIcon, getCategoryIconClass } from '@/features/categories/CategoryService';
    import { type ICategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import FormBase from '@/components/form-base/FormBase.vue';
    import { type FormProps, type FormEmits } from '@/components/form-base/FormBase';
    import WidgetCard from '@/components/widget-card/WidgetCard.vue';
    import ColorPicker from '@/components/color-picker/ColorPicker.vue';
    import IconPicker from '@/components/icon-picker/IconPicker.vue';

    const { isNew, isAutoSave, isLoading, saveAllResult, savePartialResult, deleteResult } = defineProps<FormProps>();
    const model = defineModel<ICategoryRequest>({ required: true });
    const emit = defineEmits<FormEmits<ICategoryRequest>>();

    const colorOptions: CategoryColor[] = [CategoryColor.None, CategoryColor.Blue, CategoryColor.Green, CategoryColor.Yellow, CategoryColor.Orange, CategoryColor.Red];
    const colorHexOptions: string[] = colorOptions.map(getCategoryColorHex);

    const colorHex = computed<string>({
        get: () => getCategoryColorHex(model.value.color),
        set: (hex: string) => model.value.color = getCategoryColor(hex)
    });

    const iconOptions: CategoryIcon[] = [CategoryIcon.None, CategoryIcon.Bank, CategoryIcon.Bill, CategoryIcon.House, CategoryIcon.Wallet, CategoryIcon.CreditCard, CategoryIcon.Coins, CategoryIcon.PiggyBank, CategoryIcon.Cart, CategoryIcon.Gift, CategoryIcon.Car, CategoryIcon.Plane, CategoryIcon.Restaurant];
    const iconClassOptions: string[] = iconOptions.map(getCategoryIconClass);

    const iconClass = computed<string>({
        get: () => getCategoryIconClass(model.value.icon),
        set: (value: string) => model.value.icon = getCategoryIcon(value)
    });

    // #region Events

    const formEvents = {
        onSaveAll: (data: ICategoryRequest) => emit('saveAll', data),
        onSavePartial: (id: number, data: Partial<ICategoryRequest>) => emit('savePartial', id, data),
        onDelete: (id: number) => emit('delete', id)
    };

    // #endregion Events

    // #region Form validation

    const isFormValid = (): boolean => {
        return model.value.name.trim().length > 0;
    };

    // #endregion Form validation

</script>

<template>
    <FormBase
        v-slot="{ onChange }"
        v-model="model"
        :is-new="isNew"
        :is-auto-save="isAutoSave"
        :is-loading="isLoading"
        :save-all-result="saveAllResult"
        :save-partial-result="savePartialResult"
        :delete-result="deleteResult"
        :is-form-valid="isFormValid"
        v-bind="formEvents"
    >

        <div class="row">
            <div class="col-12">
                <div class="form-body card">

                    <div class="row">
                        <div class="col-12">
                            <label class="form-label" for="name">Category Name</label>
                            <input id="name" v-model="model.name" name="Name" type="text" class="form-control" placeholder="e.g. Dining Out" required @input="onChange('name', model.name);" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <label class="form-label">Color</label>
                            <ColorPicker v-model="colorHex" :options="colorHexOptions" @change="onChange('color', model.color)" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <label class="form-label">Icon</label>
                            <IconPicker v-model="iconClass" v-color="colorHex" :options="iconClassOptions" @change="onChange('icon', model.icon)" />
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12 col-md-with-navbar-4">
                <WidgetCard icon="ruler" title="Similar Category" class="info">
                    <p class="widget-desc">You already have a similar category 'Restaurant'.</p>
                </WidgetCard>
            </div>
            <div class="col-12 col-md-with-navbar-4">
                <WidgetCard icon="wand-magic-sparkles" title="Auto-tag" class="info">
                    <p class="widget-desc"><strong>12</strong> past transactions could match this category.</p>
                </WidgetCard>
            </div>
            <div class="col-12 col-md-with-navbar-4">
                <WidgetCard icon="piggy-bank" title="Budget Impact" class="info">
                    <p class="widget-desc">Adds ~<strong>420 €</strong>/mo based on similar spending.</p>
                </WidgetCard>
            </div>
        </div>

    </FormBase>
</template>
