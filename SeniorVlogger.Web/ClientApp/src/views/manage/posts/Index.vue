<template>
    <div class="wrapper p-5 col-xl-11 col-md-12 m-auto">
        <div class="buttons pr-0 pl-0">
            <router-link tag="button" to="posts/create" class="btn btn-primary">
               <i class="fas fa-plus mr-1"></i> Create New
            </router-link>
        </div>
        
        <div class="data p-0 border border-darken-2">
            <vue-good-table :columns="columns" :rows="posts"
                :select-options="{ enabled: true, selectOnCheckboxOnly: true, selectionText: 'posts selected'}"
                :search-options="{ enabled: true }">
                <template slot="table-row" slot-scope="props">
                    <span v-if="props.column.field === 'tags'" class="tags">
                        <span class="tag" v-for="(item, index) in props.row.tags" :key="index">
                            {{ item }}
                        </span> 
                    </span>
                    <span v-else-if="props.column.field === 'mailed'" class="check-success">
                        <i class="fas fa-check text-success" v-if="props.row.mailed"></i>
                        <i class="fas fa-times text-danger" v-else></i>
                    </span>
                    <span v-else-if="props.column.field === 'scratch'" class="check-success">
                        <i class="fas fa-check text-success" v-if="props.row.scratch"></i>
                        <i class="fas fa-times text-danger" v-else></i>
                    </span>
                    <span v-else-if="props.column.field === 'buttons'" class="table-buttons">
                        <button @click="OpenPost(props.row.slug)" class="btn btn-sm btn-primary mr-2">
                            <i class="fas fa-external-link-alt"></i>
                        </button>
                        <router-link tag="button" :to="{path: `/manage/posts/edit/${props.row.id}`}" class="btn btn-sm btn-warning mr-2">
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

            <div class="controls">

            </div>
        </div>
    </div>
</template>

<script>

export default {
    data () {
        return {
            posts: [],

            columns: [
                { label: 'Title', field: 'title', sortable: false},
                { label: 'Category', field: 'category.name' },
                { 
                  label: 'Date', 
                  field: 'publishDate',
                  dateInputFormat: 'MM/dd/yyyy HH:mm:ss',
                  dateOutputFormat: 'dd.MM.yyyy',
                  type: 'date',
                  tdClass: 'vgt-left-align',
                  thClass: 'vgt-left-align'
                },
                { label: 'Tags', field: 'tags', sortable: false, globalSearchDisabled: true },
                { label: 'Mailed', field: 'mailed', sortable: false, globalSearchDisabled: true, tdClass: 'center-item' },
                { label: 'Scratch', field: 'scratch', sortable: false, globalSearchDisabled: true, tdClass: 'center-item'  },
                { label: '', field: 'buttons', sortable: false, globalSearchDisabled: true, tdClass: 'center-item' }
            ]
        }
    }, 

    methods: {
        OpenPost(slug) {
            let route = this.$router.resolve(`/blog/${slug}`)
            window.open(route.href, '_blank')
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

        async LoadPosts() {
            let response = await this.$api.get('/api/blog')
            this.posts = response.data
        }
    },

    async beforeMount() {
        await this.LoadPosts()
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
