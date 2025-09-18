<script setup lang="ts">

    import './ListView.scss';

    import { onMounted, ref } from 'vue';
    import { routes } from '@/router.ts';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { type ICategoryListResponse, type ICategoryListItemResponse } from '@/features/categories/models/ICategoryListResponse';
    import { formatAmount } from '@/features/transactions/TransactionService.ts';

    const categories = ref<ICategoryListItemResponse[]>([]);

    // Init
    onMounted(() => {
        getCategoryList();
    });

    const getCategoryList = async () => {
        apiCall<undefined, ICategoryListResponse>(`category`, { method: 'GET' })
            .then(response => {
                categories.value = response.items;
            })
            .catch(() => {
                // TODO: Add error
            });
    };

</script>

<template>
    <div class="category-list section-container container">
        <div class="category-list-items">
            <RouterLink v-for="category in categories"
                        :key="category.id"
                        :to="routes.category.update(category.id)"
                        class="category-list-item"
                        :style="{ '--color': category.colorHex }">
                <font-awesome-icon icon="fa-solid fa-plus" />
                <span class="category-list-item-name">{{ category.name }}</span>
            </RouterLink>
            <RouterLink :to="routes.category.create" class="category-list-item btn btn-outline-primary">
                <font-awesome-icon icon="fa-solid fa-plus" />
            </RouterLink>
        </div>
    </div>
</template>
