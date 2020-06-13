<template>
    <div class="wrapper" id="post-update">
        <button @click="togglePreview">
            <span v-if="preview">Hide preview</span>
            <span v-else>Show preview</span>
        </button>
        <blog-post v-show="preview" :data="data" />

        <div class="post">
            <form class="form" @submit.prevent="update">
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
                    <option v-for="post in filteredPosts"
                            :key="post.id"
                            :value="post.id">
                        {{ post.title }}
                    </option>
                </select>

                <label for="next">Previous</label>
                <select name="next" v-model="post.previous">
                    <option value="" selected>none</option>
                    <option v-for="post in filteredPosts"
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
                            <a href @click.prevent="removeFile(file)">Remove file</a>
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
        
        <client-only placeholder="Loading...">
            <TextEditor :buffer="editor.content" v-on:changed="editorTextChanged($event)" />
        </client-only>

    </div>
</template>

<script>
import graphql from '~/graphql'
import BlogPost from '~/components/BlogPost'
import FileUpload from 'vue-upload-component/dist/vue-upload-component.part.js'
import Multiselect from 'vue-multiselect'
const TextEditor = () => import('~/components/TextEditor.vue')

export default {
    middlewawre: ['auth'],
    props: {
        id: {
        type: String,
        default: null
        }
    },

    components: {
        BlogPost,
        FileUpload,
        Multiselect,
        TextEditor
    },

    data () {
        return{
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
                date: '',
                category: '',
                next: '',
                previous: '',
                content: ''
            }
        }
    },

    apollo: {
        postById: {
            query: graphql.post.getById,
            prefetch: ({ route }) => {
                return { id: route.params.id }
            },
            variables () {
                return { id: this.$route.params.id }
            },
            update (data) {
                this.$set(this, 'post', JSON.parse(JSON.stringify(data.postById)))
                this.editor.content = this.post.content
            }
        },
        posts: {
            query: graphql.post.getAll
        }
    },

    computed: {
        data () {
            return {
                ...this.post
            }
        },
        filteredPosts(){
            if(!this.posts) return null

            return this.posts.filter(post=>{
                return post.title !== this.post.title
            })
        }
    },

    methods: {
        async update () {
            await this.$apollo.mutate({
                mutation: graphql.post.update,
                variables: {
                    id: this.post.id,
                    tags: this.post.tags,
                    title: this.post.title,
                    image: this.post.image,
                    short: this.data.short,
                    content: this.editor.content,
                    scratch: this.post.scratch,
                    category: this.post.category,
                    next: this.post.next,
                    previous: this.post.previous
                }
            })
            this.$router.replace({ path: '/manage/posts' })
        },

        editorTextChanged(html){
            this.editor.content = html
            this.post.content = this.editor.content
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

        async uploadFiles (data) {
            data.headers['Content-type'] = 'multipart/form-data'
            const formData = new FormData()
            formData.append('file', data.file)
            let post = this.post;

            await this.$apollo.mutate({
                mutation: graphql.file.create,
                variables: {
                    file: data.file
                },
                update (store, result) {
                    if(!post.image){
                        post.image = result.data.fileCreate.filename
                    }
                    data.name = result.data.fileCreate.filename
                    data.graphql = result.data.fileCreate
                }
            })
        },

        async removeFile (file) {
            const vm = this
            const filename = file.name
            file.name = 'removing...'

            await this.$apollo.mutate({
                mutation: graphql.file.remove,
                variables: { id: file.graphql.id },
                update (store, result) {
                    file.name = filename,
                    vm.$set(file, 'removed', true)
                } 
            })
        },

        addTag (newTag) {
            this.tags.push(newTag)
            this.post.tags.push(newTag)
        }
    }

    
}
</script>

<style lang="sass">
    #post-update
        width: 80%
        margin: 0 auto
        input, textarea
            display: block
</style>
