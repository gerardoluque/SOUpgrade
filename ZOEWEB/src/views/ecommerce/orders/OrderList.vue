<template>
  <div class="container-fluid py-4">
    <div class="d-sm-flex justify-content-between">
      <div>
        <material-button color="success" variant="gradient">New order</material-button>
      </div>
      <div class="d-flex">
        <div class="dropdown d-inline">
          <material-button
            id="navbarDropdownMenuLink2"
            color="dark"
            variant="outline"
            class="dropdown-toggle"
            data-bs-toggle="dropdown"
            aria-expanded="false"
            >Filters</material-button
          >
          <ul
            class="dropdown-menu dropdown-menu-lg-start px-2 py-3"
            aria-labelledby="navbarDropdownMenuLink2"
          >
            <li>
              <a class="dropdown-item border-radius-md" href="javascript:;">Status: Paid</a>
            </li>
            <li>
              <a class="dropdown-item border-radius-md" href="javascript:;">Status: Refunded</a>
            </li>
            <li>
              <a class="dropdown-item border-radius-md" href="javascript:;">Status: Canceled</a>
            </li>
            <li>
              <hr class="horizontal dark my-2" />
            </li>
            <li>
              <a class="dropdown-item border-radius-md text-danger" href="javascript:;">Remove Filter</a>
            </li>
          </ul>
        </div>
        <material-button
          class="btn-icon ms-2 export"
          size
          color="dark"
          variant="outline"
          data-type="csv"
        >
          <span class="btn-inner--icon">
            <i class="ni ni-archive-2"></i>
          </span>
          <span class="btn-inner--text">Export CSV</span>
        </material-button>
      </div>
    </div>
    <div class="row">
      <div class="col-12">
        <div class="card mt-4">
          <div class="card-header">
            <h5 class="mb-0">Orders Table</h5>
            <p class="text-sm mb-0">View all the orders from the previous year.</p>
          </div>
          <div class="table-responsive">
            <table id="order-list" class="table table-flush">
              <thead class="thead-light">
                <tr>
                  <th>Id</th>
                  <th>Date</th>
                  <th>Status</th>
                  <th>Customer</th>
                  <th>Product</th>
                  <th>Revenue</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(order, index) in orders" :key="index">
                  <td>
                    <div class="d-flex align-items-center">
                      <material-checkbox />
                      <p class="text-xs font-weight-bold ms-2 mb-0">{{ order.id }}</p>
                    </div>
                  </td>
                  <td class="font-weight-bold">
                    <span class="my-2 text-xs">{{ order.date }}</span>
                  </td>
                  <td class="text-xs font-weight-bold">
                    <div class="d-flex align-items-center">
                      <material-button
                        :color="order.statusColor"
                        variant="outline"
                        class="btn-icon-only btn-rounded mb-0 me-2 btn-sm d-flex align-items-center justify-content-center"
                      >
                        <i :class="order.statusIcon" aria-hidden="true"></i>
                      </material-button>
                      <span>{{ order.status }}</span>
                    </div>
                  </td>
                  <td class="text-xs font-weight-bold">
                    <div class="d-flex align-items-center">
                      <material-avatar
                        :img="order.customerImage"
                        size="xs"
                        circular
                        class="me-2"
                        alt="user image"
                      />
                      <span>{{ order.customer }}</span>
                    </div>
                  </td>
                  <td class="text-xs font-weight-bold">
                    <span class="my-2 text-xs">{{ order.product }}</span>
                  </td>
                  <td class="text-xs font-weight-bold">
                    <span class="my-2 text-xs">{{ order.revenue }}</span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { DataTable } from "simple-datatables";
import MaterialButton from "@/components/common/MaterialButton.vue";
import MaterialAvatar from "@/components/common/MaterialAvatar.vue";
import MaterialCheckbox from "@/components/common/MaterialCheckbox.vue";
//import { useOrderStore } from "@/store/useOrderStore";
//import { storeToRefs } from "pinia";

export default {
  name: "OrderList",
  components: {
    MaterialButton,
    MaterialAvatar,
    MaterialCheckbox,
  },
  setup() {
    //const orderStore = useOrderStore();
    //const { orders } = storeToRefs(orderStore);

    return {
      //orders,
    };
  },
  mounted() {
    if (document.getElementById("order-list")) {
      const dataTableSearch = new DataTable("#order-list", {
        searchable: true,
        fixedHeight: false,
        perPageSelect: false,
      });

      document.querySelectorAll(".export").forEach(function (el) {
        el.addEventListener("click", function (el) {
          var type = el.dataset.type;

          var data = {
            type: type,
            filename: "soft-ui-" + type,
          };

          if (type === "csv") {
            data.columnDelimiter = "|";
          }

          dataTableSearch.export(data);
        });
      });
    }
  },
};
</script>