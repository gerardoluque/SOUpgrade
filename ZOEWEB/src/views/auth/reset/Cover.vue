<template>
  <navbar btn-background="bg-gradient-success" />
  <main class="mt-0 main-content">
    <div
      class="page-header align-items-start min-vh-50 m-3 border-radius-lg"
      style="
        background-image: url('https://images.unsplash.com/photo-1497996541515-6549b145cd90?ixid=MnwxMjA3fDB8MHxzZWFyY2h8MTA5fHxuYXR1cmV8ZW58MHx8MHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=800&q=60');
      "
    >
      <span class="mask bg-gradient-dark opacity-6"></span>
    </div>
    <section>
      <div class="container mb-4">
        <div class="row mt-lg-n12 mt-md-n12 mt-n12 justify-content-center">
          <div class="col-xl-4 col-lg-5 col-md-7 mx-auto">
            <div class="card mt-8">
              <div
                class="card-header p-0 position-relative mt-n4 mx-3 z-index-2"
              >
                <div
                  class="bg-gradient-warning shadow-warning border-radius-lg py-3 pe-1 text-center py-4"
                >
                  <h3 class="font-weight-bolder text-white">Reset Password</h3>
                  <p class="mb-0 text-sm text-white">
                    You will receive an e-mail in maximum 60 seconds
                  </p>
                </div>
              </div>
              <div class="py-4 card-body">
                <form role="form">
                  <div class="mb-3">
                    <material-input
                      id="email"
                      type="email"
                      label="Email"
                      placeholder="john@email.com"
                      name="email"
                      variant="static"
                    />
                  </div>
                  <div class="text-center">
                    <material-button
                      class="mt-4"
                      variant="gradient"
                      color="warning"
                      full-width
                      >Reset</material-button
                    >
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </main>
  <app-footer />
</template>

<script>
import Navbar from "@/examples/PageLayout/Navbar.vue";
import AppFooter from "@/examples/PageLayout/Footer.vue";
import MaterialInput from "@/components/common/MaterialInput.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { useMainStore } from "@/store/useMainStore";

export default {
  name: "ResetCover",
  components: {
    Navbar,
    AppFooter,
    MaterialInput,
    MaterialButton,
  },
  setup() {
    const store = useMainStore();

    const toggleDisplay = () => {
      store.toggleEveryDisplay();
      store.toggleHideConfig();
    };

    return { toggleDisplay };
  },
  beforeMount() {
    this.toggleDisplay();
    document.body.classList.remove("bg-gray-100");
  },
  beforeUnmount() {
    this.toggleDisplay();
    document.body.classList.add("bg-gray-100");
  },
};
</script>