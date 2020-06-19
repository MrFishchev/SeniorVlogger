<template>
    <div class="wrapper col-lg-7 col-md-10 col-sm-12" id="post-create">
        <div class="col-12 p-0 mt-5 mb-5">
            <button class="btn btn-primary text-white w-100" @click="togglePreview">
                <span v-if="preview">Hide preview</span>
                <span v-else>Show preview</span>
            </button>
        </div>
        <blog-post v-show="preview" :data="data" />

        <div class="post" v-show="!preview">
            <form class="form" @submit.prevent="publish">
                <div class="form-group">
                    <label for="title">Title</label>
                    <input type="text" placeholder="Title" id="title" v-model="post.title" class="form-control" required/>
                </div>

                <div class="form-group">
                    <label for="description">Description</label>
                    <textarea id="description" v-model="post.description" class="form-control" rows="3" required/>
                </div>

                <div class="form-group">
                    <label for="category">Category</label>
                    <select id="category" v-model="post.category" class="form-control" required>
                        <option value="" selected>Select Category</option>
                        <option v-for="category in categories"
                                :key="category.id" :value="category.id">
                            {{category.name}}
                        </option>
                    </select>
                </div>

                <div class="form-group">
                    <label for="tags">Tags</label>
                    <multiselect class="tags"
                        tag-placeholder="Add this as new tag" 
                        placeholder="Search or add a tag" 
                        :options="post.tags" 
                        :multiple="true" 
                        :taggable="true" 
                        @tag="addTag">
                    </multiselect>
                </div>

                <div class="form-group">
                    <label>Post Image</label>
                    <div class="custom-file">
                        <input type="file" class="custom-file-input" id="customFile">
                        <label class="custom-file-label" for="customFile">{{post.image || 'Select Image'}}</label>
                    </div>
                </div>

                <div class="form-group">
                    <label for="previous">Previous Post</label>
                    <select id="previous" v-model="post.previous" class="form-control">
                        <option value="" selected>Optional</option>
                        <option v-for="post in posts"
                                :key="post.id" :value="post.id">
                            {{post.name}}
                        </option>
                    </select>
                </div>

                <div class="form-group">
                    <label for="next">Next Post</label>
                    <select id="next" v-model="post.next" class="form-control">
                        <option value="" selected>Optional</option>
                        <option v-for="post in posts"
                                :key="post.id" :value="post.id">
                            {{post.name}}
                        </option>
                    </select>
                </div>
                
                <div class="form-group">
                    <div class="form-check">
                        <input class="form-check-input" v-model="post.mailed"
                            type="checkbox" id="mailed">
                        <label class="form-check-label" for="mailed">
                        Send Mail to Subscribers
                        </label>
                    </div>
                </div>

                <div class="form-group">
                    <div class="form-check">
                        <input class="form-check-input" v-model="post.scratch"
                            type="checkbox" id="scratch">
                        <label class="form-check-label" for="scratch">
                        Scratch (Don't publish now)
                        </label>
                    </div>
                </div>

                <TextEditor class="editor" :buffer="editor.content" v-on:changed="editorTextChanged($event)" />

                <div class="buttons mb-5">
                    <button class="btn btn-success w-25 text-white">Save Blog</button>
                    <button class="btn btn-danger w-25 text-white">Cancel</button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
import BlogPost from '@/components/BlogPost'
import Multiselect from 'vue-multiselect'
const TextEditor = () => import('@/components/TextEditor.vue')

export default {
    components: {
        BlogPost,
        Multiselect,
        TextEditor
    },

    data () {
        return {
            preview: false,

            editor:{
                content: '',
            },

            post: {
                title: '',
                description: '',
                tags: [],
                image: '',
                date: '',
                category: 0,
                next: 0,
                previous: 0,
                mailed: false,
                scratch: true
            },

            posts: [],
            categories: []
        }
    },

    beforeMount() {

    },

    computed: {
        data () {
            this.post.content = this.editor.content
            return {
                ...this.post
            }
        }
    },

    methods: {
        editorTextChanged(html){
            this.editor.content = html
        },

        formatDate(date) {
            var monthNames = [
                "January", "February", "March",
                "April", "May", "June", "July",
                "August", "September", "October",
                "November", "December"
            ]

            var day = date.getDate()
            var monthIndex = date.getMonth()
            var year = date.getFullYear()

            return monthNames[monthIndex] + ' ' + day + ', ' + year
        },

        togglePreview () {
            this.preview = !this.preview
        },

        inputFilter (newFile, oldFile, prevent) {
            if(newFile && !oldFile){
                 if (/(\/|^)(Thumbs\.db|desktop\.ini|\..+)$/.test(newFile.name)) {
                    return prevent()
                }
                if (/\.(php?|html?|jsx?).*$/i.test(newFile.name)) {
                    return prevent()
                }
            }
        },

        async publish() {

            let json = {
                id: 1,
                title: 'hello'
            }
            this.$api.post('/api/Blog/Create', json).then(res => console.log(res))

            // await this.$apollo.mutate({
            //     mutation: graphql.post.create,
            //     variables: {
            //         tags: this.post.tags,
            //         title: this.post.title,
            //         image: this.post.image,
            //         thumb: this.post.thumb,
            //         short: this.post.short,
            //         content: this.data.content,
            //         scratch: this.post.scratch,
            //         category: this.post.category
            //         next: this.post.next,
            //         previous: this.post.previous,
            //         date: this.post.date
            //     },
            //     update (store, result) {
            //         const data = store.readQuery({ query: graphql.post.getAll })
            //         store.writeQuery({ query: graphql.post.getAll, data })
            //     }
            // })
            this.$router.replace({ path: '/manage/posts' })
        },

        addTag (newTag) {
            this.post.tags.push(newTag)
        }
    }
}
</script>

<style lang="sass">
    #post-create
        margin: 0 auto
        .editor 
            margin: 30px 0 30px 0 
            .ql-editor
                min-height: 300px
        .buttons
            display: flex
            justify-content: space-evenly
            align-items: center
</style>
<style src="vue-multiselect/dist/vue-multiselect.min.css"></style>