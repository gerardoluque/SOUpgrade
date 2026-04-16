import { createRouter, createWebHistory } from "vue-router";
import Default from "../views/dashboards/Default.vue";
import Sales from "../views/dashboards/Sales.vue";
import Overview from "../views/pages/profile/Overview.vue";
import Projects from "../views/pages/profile/Projects.vue";
import Timeline from "../views/pages/projects/Timeline.vue";
import Pricing from "../views/pages/Pricing.vue";
import RTL from "../views/pages/Rtl.vue";
import Charts from "../views/pages/Charts.vue";
import Notifications from "../views/pages/Notifications.vue";
import Kanban from "../views/applications/Kanban.vue";
import Wizard from "../views/applications/wizard/Wizard.vue";
import DataTables from "../views/applications/DataTables.vue";
import Calendar from "../views/applications/Calendar.vue";
import NewProduct from "../views/ecommerce/products/NewProduct.vue";
import EditProduct from "../views/ecommerce/products/EditProduct.vue";
import ProductPage from "../views/ecommerce/products/ProductPage.vue";
import OrderDetails from "../views/ecommerce/orders/OrderDetails";
import OrderList from "../views/ecommerce/orders/OrderList";
import NewUser from "../views/pages/users/NewUser.vue";
import Settings from "../views/pages/account/Settings.vue";
import Billing from "../views/pages/account/Billing.vue";
import Invoice from "../views/pages/account/Invoice.vue";
import Widgets from "../views/pages/Widgets.vue";
import Basic from "../views/auth/signin/Basic.vue";
import Cover from "../views/auth/signin/Cover.vue";
import Illustration from "../views/auth/signin/Illustration.vue";
import ResetCover from "../views/auth/reset/Cover.vue";
import SignupCover from "../views/auth/signup/Cover.vue";
import UserInfo from "../views/pages/users/UserInfo.vue";
import { msalInstance } from "../authConfig";
 
const requireAuth = (to, from, next) => {
  const account = msalInstance.getActiveAccount();
  if (!account) {
    msalInstance.loginRedirect({
      scopes: ["User.Read"], // Cambia los scopes según tus necesidades
    });
  } else {
    next();
  }
};

const routes = [
  {
    path: "/",
    name: "/",
    redirect: "/authentication/signin/basic",
  },
  {
    path: "/pages/users/user-info",
    name: "UserInfo",
    component: UserInfo,
    beforeEnter: requireAuth,
  },
  {
    path: "/dashboards/dashboard-default",
    name: "Default",
    component: Default,
    beforeEnter: requireAuth,
  },
  {
    path: "/dashboards/sales",
    name: "Sales",
    component: Sales,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/profile/overview",
    name: "Profile Overview",
    component: Overview,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/profile/projects",
    name: "All Projects",
    component: Projects,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/projects/timeline",
    name: "Timeline",
    component: Timeline,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/pricing-page",
    name: "Pricing Page",
    component: Pricing,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/rtl-page",
    name: "RTL",
    component: RTL,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/charts",
    name: "Charts",
    component: Charts,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/widgets",
    name: "Widgets",
    component: Widgets,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/notifications",
    name: "Notifications",
    component: Notifications,
    beforeEnter: requireAuth,
  },
  {
    path: "/applications/kanban",
    name: "Kanban",
    component: Kanban,
    beforeEnter: requireAuth,
  },
  {
    path: "/applications/wizard",
    name: "Wizard",
    component: Wizard,
    beforeEnter: requireAuth,
  },
  {
    path: "/applications/data-tables",
    name: "Data Tables",
    component: DataTables,
    beforeEnter: requireAuth,
  },
  {
    path: "/applications/calendar",
    name: "Calendar",
    component: Calendar,
    beforeEnter: requireAuth,
  },
  {
    path: "/ecommerce/products/new-product",
    name: "New Product",
    component: NewProduct,
    beforeEnter: requireAuth,
  },
  {
    path: "/ecommerce/products/edit-product",
    name: "Edit Product",
    component: EditProduct,
    beforeEnter: requireAuth,
  },
  {
    path: "/ecommerce/products/product-page",
    name: "Product Page",
    component: ProductPage,
    beforeEnter: requireAuth,
  },
  {
    path: "/ecommerce/Orders/order-details",
    name: "Order Details",
    component: OrderDetails,
    beforeEnter: requireAuth,
  },
  {
    path: "/ecommerce/Orders/order-list",
    name: "Order List",
    component: OrderList,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/users/new-user",
    name: "New User",
    component: NewUser,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/account/settings",
    name: "Settings",
    component: Settings,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/account/billing",
    name: "Billing",
    component: Billing,
    beforeEnter: requireAuth,
  },
  {
    path: "/pages/account/invoice",
    name: "Invoice",
    component: Invoice,
    beforeEnter: requireAuth,
  },
  {
    path: "/authentication/signin/basic",
    name: "Signin Basic",
    component: Basic ,
  },
  {
    path: "/authentication/signin/cover",
    name: "Signin Cover",
    component: Cover, 
  },
  {
    path: "/authentication/signin/illustration",
    name: "Signin Illustration",
    component: Illustration,
  },
  {
    path: "/authentication/reset/cover",
    name: "Reset Cover",
    component: ResetCover ,
  },
  {
    path: "/authentication/signup/cover",
    name: "Signup Cover",
    component: SignupCover,beforeEnter: requireAuth,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
  linkActiveClass: "active",
});

export default router;
