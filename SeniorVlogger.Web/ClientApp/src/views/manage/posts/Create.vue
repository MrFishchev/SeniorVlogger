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
            <form class="form" @submit.prevent="publish" enctype="multipart/form-data">
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
                        <option value="" selected disabled>Select Category</option>
                        <option v-for="category in categories"
                                :key="category.id" :value="category">
                            {{ category.name }}
                        </option>
                    </select>
                </div>

                <div class="form-group">
                    <label for="tags">Tags</label>
                    <multiselect class="tags"
                        tag-placeholder="Add this as new tag" 
                        placeholder="Search or add a tag" 
                        label="name"
                        track-by="code"
                        v-model="tagValues"
                        :options="tagOptions" 
                        :multiple="true" 
                        :taggable="true" 
                        @tag="addTag">
                    </multiselect>
                </div>

                <div class="form-group">
                    <label>Post Image</label>
                    <div class="custom-file">
                        <input type="file" class="custom-file-input" id="customFile" @change="selectImage" ref="image" required>
                        <label class="custom-file-label" for="customFile">{{(postImage != null) ? postImage.name : null || 'Select Image'}}</label>
                    </div>
                </div>

                <div class="form-group">
                    <label for="previous">Previous Post</label>
                    <select id="previous" v-model="post.previous" class="form-control">
                        <option value="" selected>Optional</option>
                        <option v-for="post in posts"
                                :key="post.id" :value="post">
                            {{post.name}}
                        </option>
                    </select>
                </div>

                <div class="form-group">
                    <label for="next">Next Post</label>
                    <select id="next" v-model="post.next" class="form-control">
                        <option value="" selected>Optional</option>
                        <option v-for="post in posts"
                                :key="post.id" :value="post">
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

            editor: {
                content: '',
            },

            postImage: null,
            post: {
                title: '',
                description: '',
                tags: [],
                imageUrl: '',
                category: null,
                next: null,
                previous: null,
                mailed: false,
                scratch: true
            },

            tagValues: [],
            tagOptions: [],

            posts: [],
            categories: [{id: 1, name: 'cat'}]
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
        selectImage(event){
            this.postImage = event.target.files[0]
        },

        editorTextChanged(html){
            this.editor.content = html
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

        async uploadImage() {
            if (this.postImage == null) return

            var formData = new FormData()
            var imagefile = this.postImage
            formData.append("image", imagefile)

            return this.$api({
                method: 'post',
                url: '/api/blog/image',
                data: formData,
                config: { headers: {'Content-Type': 'multipart/form-data'}}
            })
        },

        async publish() {
            let response = await this.uploadImage()
            this.post.imageUrl = response.data
            this.post.tags = this.tagValues.map(i => { return i.name })

            this.$api.post('/api/blog', this.post)
              .then(res => this.$router.push({ path: '/manage/posts' }))
              .catch(error => {
                  console.log('Cannot save post, deleting image')
                  this.DeleteImage()
              })
        },

        DeleteImage() {
            this.$api.delete('/api/blog/image', { data: { path: this.post.imageUrl } })
        },

        addTag(newTag) {
            let tag = {
                name: newTag,
                code: newTag.substring(0, 2) + Math.floor((Math.random() * 10000000))
            }
            this.tagOptions.push(tag)
            this.tagValues.push(tag)
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