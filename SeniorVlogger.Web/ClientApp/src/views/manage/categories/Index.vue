<template>
    <div class="wrapper p-5 col-xl-11 col-md-12 m-auto">
        <div class="buttons pr-0 pl-0">
            <router-link tag="button" to="categories/create" class="btn btn-primary">
               <i class="fas fa-plus mr-1"></i> Create New
            </router-link>
        </div>
        
        <div class="data p-0 border border-darken-2">
            <vue-good-table :columns="columns" :rows="categories"
                :select-options="{ enabled: true, selectOnCheckboxOnly: true, selectionText: 'categories selected'}"
                :search-options="{ enabled: true }">
                <template slot="table-row" slot-scope="props">
                    <span v-if="props.column.field == 'buttons'" class="table-buttons">
                        <router-link tag="button" :to="{path: `/manage/categories/edit/${props.row.id}`}" class="btn btn-sm btn-warning mr-2">
                            <i class="far fa-edit"></i>
                        </router-link >
                        <button class="btn btn-sm btn-danger" @click="OnDelete(props.row.id)">
                            <i class="fas fa-trash"></i>
                        </button>
                    </span>
                    <span v-else>
                        {{props.formattedRow[props.column.field]}}
                    </span>
                </template>
            </vue-good-table>
        </div>
    </div>
</template>

<script>

export default {
    data () {
        return {
            categories: [],

            columns: [
                { label: 'Name', field: 'name'},
                { label: '', field: 'buttons', sortable: false, globalSearchDisabled: true, tdClass: 'center-item' }
            ]
        }
    }, 

    methods: {

        async OnDelete(id) {
            let res = await this.$swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to restore the category!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                customClass: {
                    confirmButton: 'btn btn-danger mr-3',
                    cancelButton: 'btn btn-light'
                },
                buttonsStyling: false
            })

            if(res.isConfirmed){
                let response = await this.$api.delete(`/api/category/${id}`)
                if(response.status === 200 && response.data.status){
                    this.$notify({
                        type: 'success',
                        text: 'Category was deleted',
                        group: 'app',
                    })
                    this.LoadCategories()
                }
                else{
                    this.$notify({
                        type: 'error',
                        title: 'Error',
                        text: response.data.message,
                        group: 'app',
                    })
                }
               
            }
        },

        async LoadCategories() {
            let response = await this.$api.get('/api/category')
            this.categories = response.data
        }
    },

    async beforeMount() {
      await this.LoadCategories()
    }
}
</script>

<style lang="sass">
    .wrapper
        td.center-item
            text-align: center
            vertical-align: middle
        & > .buttons
            display: flex
            justify-content: flex-end
            padding-bottom: 20px
        .data
            .table-buttons
                display: flex
                justify-content: center
            .controls
                background: linear-gradient(#f4f5f8,#f1f3f6)
                height: 40px

</style>
