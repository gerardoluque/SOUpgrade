const route2Factor = [
  {
    path: "/auth/external/2Factor",
    name: "External2Factor",
    component: () => import("@/views/auth/external/2Factor/views/External2Factor.vue"),    
  },   
];

export default route2Factor;