<template>
  <div class="py-4 container-fluid">
    <div class="m-3 d-flex">
      <div class="ms-auto d-flex">
        <div class="mt-1 pe-4 position-relative">
          <p class="mb-2 text-xs text-secondary font-weight-bold">
            Team members:
          </p>
          <div class="d-flex align-items-center justify-content-center">
            <div class="avatar-group">
              <a
                href="javascript:;"
                class="avatar avatar-sm rounded-circle"
                data-toggle="tooltip"
                data-original-title="Jessica Rowland"
              >
                <img alt="Image placeholder" src="../../assets/img/team-1.jpg" />
              </a>
              <a
                href="javascript:;"
                class="avatar avatar-sm rounded-circle"
                data-toggle="tooltip"
                data-original-title="Audrey Love"
              >
                <img
                  alt="Image placeholder"
                  src="../../assets/img/team-2.jpg"
                  class="rounded-circle"
                />
              </a>
              <a
                href="javascript:;"
                class="avatar avatar-sm rounded-circle"
                data-toggle="tooltip"
                data-original-title="Michael Lewis"
              >
                <img
                  alt="Image placeholder"
                  src="../../assets/img/team-3.jpg"
                  class="rounded-circle"
                />
              </a>
              <a
                href="javascript:;"
                class="avatar avatar-sm rounded-circle"
                data-toggle="tooltip"
                data-original-title="Lucia Linda"
              >
                <img
                  alt="Image placeholder"
                  src="../../assets/img/team-4.jpg"
                  class="rounded-circle"
                />
              </a>
              <a
                href="javascript:;"
                class="avatar avatar-sm rounded-circle"
                data-toggle="tooltip"
                data-original-title="Ronald Miller"
              >
                <img
                  alt="Image placeholder"
                  src="../../assets/img/team-5.jpg"
                  class="rounded-circle"
                />
              </a>
            </div>
          </div>
          <hr class="mt-0 vertical dark" />
        </div>
        <div class="ps-4">
          <button
            class="mt-3 mb-0 btn bg-gradient-success btn-icon-only"
            data-toggle="modal"
            data-target="#new-board-modal"
          >
            <i class="fa fa-plus"></i>
          </button>
        </div>
      </div>
    </div>
    <div class="mt-3 kanban-container">
      <div class="py-2 min-vh-100 d-inline-flex" style="overflow-x: auto">
        <div id="myKanban"></div>
      </div>
    </div>
    <!-- Modals for new board and task details remain unchanged -->
  </div>
</template>

<script>
/* eslint-disable */
import "jkanban/dist/jkanban.min.js";
import "jkanban/dist/jkanban.min.css";
import { useMainStore } from "@/store/useMainStore";

export default {
  name: "Kanban",
  setup() {
    const store = useMainStore();

    const initializeKanban = () => {
      if (document.getElementById("myKanban")) {
        const KanbanTest = new jKanban({
          element: "#myKanban",
          gutter: "10px",
          addItemButton: true,
          buttonContent: "+",
          widthBoard: "450px",
          click: (el) => {
            const jkanbanInfoModal = document.getElementById(
              "jkanban-info-modal"
            );

            const jkanbanInfoModalTaskId = document.querySelector(
              "#jkanban-info-modal #jkanban-task-id"
            );
            const jkanbanInfoModalTaskTitle = document.querySelector(
              "#jkanban-info-modal #jkanban-task-title"
            );
            const jkanbanInfoModalTaskAssignee = document.querySelector(
              "#jkanban-info-modal #jkanban-task-assignee"
            );
            const jkanbanInfoModalTaskDescription = document.querySelector(
              "#jkanban-info-modal #jkanban-task-description"
            );

            const taskId = el.getAttribute("data-eid");
            const taskTitle = el.querySelector("p.text").innerHTML;
            const taskAssignee = el.getAttribute("data-assignee");
            const taskDescription = el.getAttribute("data-description");

            jkanbanInfoModalTaskId.value = taskId;
            jkanbanInfoModalTaskTitle.value = taskTitle;
            jkanbanInfoModalTaskAssignee.value = taskAssignee;
            jkanbanInfoModalTaskDescription.value = taskDescription;

            const myModal = new bootstrap.Modal(jkanbanInfoModal, {
              show: true,
            });
            myModal.show();
          },
          buttonClick: (el, boardId) => {
            if (
              document.querySelector(
                `[data-id='${boardId}'] .itemform`
              ) === null
            ) {
              const formItem = document.createElement("form");
              formItem.setAttribute("class", "itemform");
              formItem.innerHTML = `
                <div class="form-group">
                  <textarea class="form-control" rows="2" autofocus></textarea>
                </div>
                <div class="form-group">
                  <button type="submit" class="btn bg-gradient-success btn-sm pull-end">Add</button>
                  <button type="button" id="kanban-cancel-item" class="btn bg-gradient-light btn-sm pull-end me-2">Cancel</button>
                </div>`;

              KanbanTest.addForm(boardId, formItem);

              formItem.addEventListener("submit", (e) => {
                e.preventDefault();
                const text = e.target[0].value;
                const newTaskId =
                  "_" + text.toLowerCase().replace(/ /g, "_") + boardId;
                KanbanTest.addElement(boardId, {
                  id: newTaskId,
                  title: text,
                  class: ["border-radius-md"],
                });
                formItem.parentNode.removeChild(formItem);
              });

              document.getElementById("kanban-cancel-item").onclick = () => {
                formItem.parentNode.removeChild(formItem);
              };
            }
          },
          boards: [
            {
              id: "_backlog",
              title: "Backlog",
              item: [
                {
                  id: "_task_1_title_id",
                  title: '<p class="mb-0 text">Click me to change title</p>',
                  class: ["border-radius-md"],
                },
                {
                  id: "_task_2_title_id",
                  title:
                    '<p class="mb-0 text">Drag me to "In progress" section</p>',
                  class: ["border-radius-md"],
                },
              ],
            },
            {
              id: "_progress",
              title: "In progress",
              item: [
                {
                  id: "_task_3_title_id",
                  title:
                    '<span class="mt-2 badge badge-sm bg-gradient-warning">Errors</span><p class="mt-2 text">Fix Firefox errors</p>',
                  class: ["border-radius-md"],
                },
              ],
            },
          ],
        });

        const addBoardDefault = document.getElementById("jkanban-add-new-board");
        addBoardDefault.addEventListener("click", () => {
          const newBoardName = document.getElementById(
            "jkanban-new-board-name"
          ).value;
          const newBoardId = "_" + newBoardName.toLowerCase().replace(/ /g, "_");
          KanbanTest.addBoards([
            {
              id: newBoardId,
              title: newBoardName,
              item: [],
            },
          ]);
          document.querySelector("#new-board-modal").classList.remove("show");
          document.querySelector("body").classList.remove("modal-open");
          document.querySelector(".modal-backdrop").remove();
        });

        const updateTask = document.getElementById("jkanban-update-task");
        updateTask.addEventListener("click", () => {
          const jkanbanInfoModalTaskId = document.querySelector(
            "#jkanban-info-modal #jkanban-task-id"
          );
          const jkanbanInfoModalTaskTitle = document.querySelector(
            "#jkanban-info-modal #jkanban-task-title"
          );
          const jkanbanInfoModalTaskAssignee = document.querySelector(
            "#jkanban-info-modal #jkanban-task-assignee"
          );
          const jkanbanInfoModalTaskDescription = document.querySelector(
            "#jkanban-info-modal #jkanban-task-description"
          );

          KanbanTest.replaceElement(jkanbanInfoModalTaskId.value, {
            title: jkanbanInfoModalTaskTitle.value,
            assignee: jkanbanInfoModalTaskAssignee.value,
            description: jkanbanInfoModalTaskDescription.value,
          });

          document.querySelector("#jkanban-info-modal").classList.remove("show");
          document.querySelector("body").classList.remove("modal-open");
          document.querySelector(".modal-backdrop").remove();
        });
      }
    };

    return { store, initializeKanban };
  },
  mounted() {
    this.initializeKanban();
  },
};
</script>