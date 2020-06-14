<template>
    <div class="wrapper" id="post-create">
        <button @click="togglePreview">
            <span v-if="preview">Hide preview</span>
            <span v-else>Show preview</span>
        </button>
        <blog-post v-show="preview" :data="data" />

        <div class="post">
            <form class="form" @submit.prevent="publish">
                <input type="text" placeholder="Title"
                    v-model="post.title" />
                <input type="text" placeholder="Image"
                    v-model="post.image" />
                <input type="text" placeholder="Thumbnail"
                    v-model="post.thumb" />
                <input type="text" placeholder="Short"
                    v-model="post.short" />
                <input type="text" placeholder="Category"
                    v-model="post.category" />

                <label for="next">Next</label>
                <select name="next" v-model="post.next">
                    <option value="" selected>none</option>
                    <option v-for="post in posts"
                            :key="post.id"
                            :value="post.id">
                        {{ post.title }}
                    </option>
                </select>

                <label for="next">Previous</label>
                <select name="next" v-model="post.previous">
                    <option value="" selected>none</option>
                    <option v-for="post in posts"
                            :key="post.id"
                            :value="post.id">
                        {{ post.title }}
                    </option>
                </select>

                <multiselect v-model="post.tags"
                    :close-on-select="false"
                    :clear-on-select="false"
                    :preserve-search="true"
                    :preselect-first="false"
                    :options="tags"
                    :multiple="true"
                    :taggable="true"
                    tag-placeholder="Add this as new tag"
                    placeholder="Search or add a tag"
                    @tag="addTag" />

                <label for="scratch">Scratch</label>
                <input type="checkbox" name="scratch"
                    v-model="post.scratch" />
                
                <ul>
                    <li v-for="file in files" :key="file.id">
                        <span v-if="file.success && !file.removed">
                            <a href="#" @click.prevent="removeFile(file)">Remove file</a>
                        </span>
                        <span v-if="!file.removed">{{file.name}}</span>
                        <span v-if="!file.removed">{{file.size}}</span>
                        <span v-else>-- removed --</span>
                        <span v-if="file.active">uploading...</span>
                        <span v-else-if="file.error">error</span>
                        <span v-else />
                    </li>
                </ul>
                <file-upload v-model="files"
                    ref="upload"
                    :multiple="false"
                    :custom-action="uploadFiles"
                    extensions="jpg,jpeg,png"
                    accept="image/png,image/jpeg,image"
                    @input-filter="inputFilter">
                    Select files
                </file-upload>
                <button v-if="!$refs.upload || !$refs.upload.active"
                    type="button"
                    @click.prevent="$refs.upload.active = true">
                    Upload
                </button>
                <button v-else
                    type="button"
                    @click.prevent="$refs.upload.active = false">
                    Stop uploading
                </button>

                <button type="submit">Submit</button>

            </form>
        </div>
        
        <TextEditor :buffer="editor.content" v-on:changed="editorTextChanged($event)" />
    </div>
</template>

<script>
import BlogPost from '@/components/BlogPost'
import FileUpload from 'vue-upload-component/dist/vue-upload-component.part.js'
import Multiselect from 'vue-multiselect'
const TextEditor = () => import('@/components/TextEditor.vue')

export default {
    // middlewawre: ['auth'],
    components: {
        BlogPost,
        FileUpload,
        Multiselect,
        TextEditor
    },

    data () {
        return {
            editor:{
                content: '',
            },
            files: [],
            tags: [],
            preview: false,
            post: {
                title: '',
                short: '',
                tags: [],
                image: '',
                thumb: '',
                date: this.formatDate(new Date()),
                category: '',
                next: '',
                previous: ''
            },
            posts: []
        }
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
            this.$http.post('/Admin/Blog/Create', json).then(res => console.log(res))

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
            //         category: this.post.category.toLowerCase(),
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

        async uploadFiles (data) {
            data.headers['Content-type'] = 'multipart/form-data'
            const formData = new FormData()
            formData.append('file', data.file)
            let post = this.post;

            // await this.$apollo.mutate({
            //     mutation: graphql.file.create,
            //     variables: {
            //         file: data.file
            //     },
            //     update (store, result) {
            //         if(!post.image){
            //             post.image = result.data.fileCreate.filename
            //         }
            //         data.name = result.data.fileCreate.filename
            //         data.graphql = result.data.fileCreate
            //     }
            // })
        },

        async removeFile (file) {
            const vm = this
            const filename = file.name
            file.name = 'removing...'

            // await this.$apollo.mutate({
            //     mutation: graphql.file.remove,
            //     variables: { id: file.graphql.id },
            //     update (store, result) {
            //         file.name = filename,
            //         vm.$set(file, 'removed', true)
            //     } 
            // })
        },

        addTag (newTag) {
            this.tags.push(newTag)
            this.post.tags.push(newTag)
        }
    }
}
</script>

<style lang="sass">
    #post-create
        width: 80%
        margin: 0 auto
        input, textarea
            display: block
</style>
