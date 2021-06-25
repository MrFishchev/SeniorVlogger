<template>
    <div class="wrapper p-5 col-xl-11 col-md-12 m-auto">
        <div class="buttons pr-0 pl-0">
            <router-link tag="button" to="users/create" class="btn btn-primary">
               <i class="fas fa-plus mr-1"></i> Create New
            </router-link>
        </div>
        
        <div class="data p-0 border border-darken-2">
            <vue-good-table :columns="columns" :rows="users"
                :select-options="{ enabled: true, selectOnCheckboxOnly: true, selectionText: 'posts selected'}"
                :search-options="{ enabled: true }">
                <template slot="table-row" slot-scope="props">
                    <span v-if="props.column.field === 'emailConfirmed'" class="check-success">
                        <i class="fas fa-check text-success" v-if="props.row.emailConfirmed"></i>
                        <i class="fas fa-times text-danger" v-else></i>
                    </span>
                    <span v-else-if="props.column.field === 'isLocked'" class="check-success">
                        <i class="fas fa-check text-success" v-if="props.row.isLocked"></i>
                        <i class="fas fa-times text-danger" v-else></i>
                    </span>
                    <span v-else-if="props.column.field === 'buttons'" class="table-buttons">
                        <router-link tag="button" :to="{path: `/manage/users/edit/${props.row.id}`}" class="btn btn-sm btn-secondary mr-2">
                            <i class="far fa-edit"></i>
                        </router-link >
                        <button @click="OnLock(props.row.id)" :class="['btn', 'btn-sm', 'mr-2', props.row.isLocked ? 'btn-success':'btn-primary']">
                            <i class="fas fa-lock-open" v-if="props.row.isLocked"></i>
                            <i class="fas fa-lock" v-else></i>
                        </button>
                        <button class="btn btn-sm btn-danger" @click="OnDelete(props.row.id)">
                            <i class="fas fa-trash"></i>
                        </button>
                    </span>
                    <span v-else>
                        {{props.formattedRow[props.column.field]}}
                    </span>
                </template>
            </vue-good-table>

            <div class="controls">

            </div>
        </div>
    </div>
</template>

<script>

export default {
    data () {
        return {
            users: [],

            columns: [
              { label: 'Email', field: 'email'},
              { label: 'Email Confirmed', field: 'emailConfirmed', sortable: false, globalSearchDisabled: true, tdClass: 'center-item' },
              { label: 'Locked', field: 'isLocked', sortable: false, globalSearchDisabled: true, tdClass: 'center-item' },
              { label: 'Role', field: 'role'},
              { label: '', field: 'buttons', sortable: false, globalSearchDisabled: true, tdClass: 'center-item' }
            ]
        }
    }, 

    methods: {
        async OnLock(id){
          
        },
      
        async OnDelete(id) {
            let res = await this.$swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to restore the post!",
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
                let response = await this.$api.delete(`/api/blog/${id}`)
                if(response.status === 200){
                    this.$notify({
                        type: 'success',
                        text: 'Post was deleted',
                        group: 'app',
                    })
                    this.LoadPosts()
                }
                else{
                    this.$notify({
                        type: 'error',
                        title: 'Error',
                        text: 'Cannot delete post',
                        group: 'app',
                    })
                }
               
            }
        },

        async LoadUsers() {
            let response = await this.$api.get('/api/user/all')
            this.users = response.data
            console.log(this.user)
        }
    },

    async beforeMount() {
        await this.LoadUsers()
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
        .tag
          display: inline-block
          background: #e0ecff
          padding: 5px
          margin: 2px
          color: #8c8c8c

</style>
