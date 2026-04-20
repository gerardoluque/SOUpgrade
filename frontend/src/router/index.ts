import { createRouter, createWebHistory } from 'vue-router'
import DashboardView from '../views/DashboardView.vue'
import ServiceOrdersView from '../views/ServiceOrdersView.vue'
import ServiceOrderDetailView from '../views/ServiceOrderDetailView.vue'
import CreateServiceOrderView from '../views/CreateServiceOrderView.vue'
import EditServiceOrderView from '../views/EditServiceOrderView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', name: 'dashboard', component: DashboardView },
    { path: '/service-orders', name: 'service-orders', component: ServiceOrdersView },
    { path: '/service-orders/create', name: 'create-service-order', component: CreateServiceOrderView },
    { path: '/service-orders/:id', name: 'service-order-detail', component: ServiceOrderDetailView },
    { path: '/service-orders/:id/edit', name: 'edit-service-order', component: EditServiceOrderView },
    { path: '/:pathMatch(.*)*', redirect: '/' },
  ],
})

export default router
