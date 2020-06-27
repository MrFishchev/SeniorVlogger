<template>
    <div class="wrapper col-lg-7 col-md-10 col-sm-12" id="category-create">
        <div class="category p-5">
            <form class="form" @submit.prevent="Publish" enctype="multipart/form-data">
                <div class="form-group">
                    <label for="name">Name</label>
                    <input type="text" placeholder="Name" id="name" v-model="category.name" class="form-control" required/>
                </div>

                <div class="buttons mb-5">
                    <button class="btn btn-success w-25 text-white" type="submit">{{ editMode ? 'Update Category' : 'Create Category'}}</button>
                    <router-link to="/manage/categories" tag="button" class="btn btn-danger w-25 text-white" @click.prevent="this.$router.push('/')">Cancel</router-link>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            editMode: false,
            category: {
                id: 0,
                name: ''
            }
        }
    },

    beforeMount() {
        let id = this.$route.params.id
        if(id){
            this.category.id = id
            this.editMode = true
            this.LoadEditingCategory(id)
        }
    },

    methods: {

        LoadEditingCategory(id){
            this.$api.get(`/api/category/${id}`)
            .then(response => {
                this.category = response.data
            })
            .catch(error => {
                this.$notify({
                    type: 'error',
                    title: 'Error',
                    text: 'Cannot load the category',
                    group: 'app',
                })
            })
        },

        async Publish() {
            if(this.editMode){
                this.UpdateCategory()
                return
            }

            this.$api.post('/api/category', this.category)
                .then(res => this.$router.push({ path: '/manage/categories' }))
                .catch(error => {
                    this.$notify({
                        type: 'error',
                        title: 'Error',
                        text: 'Cannot save the category',
                        group: 'app',
                    })
                })
        },

        UpdateCategory() {
            this.$api.put('/api/category', this.category)
            .then(res => this.$router.push({path: '/manage/categories'}))
            .catch(error => {
                this.$notify({
                    type: 'error',
                    title: 'Error',
                    text: 'Cannot update the category',
                    group: 'app',
                })
            })
        }

    }
}
</script>

<style lang="sass">
    #category-create
        margin: 0 auto
        .buttons
            display: flex
            justify-content: space-evenly
            align-items: center
</style>