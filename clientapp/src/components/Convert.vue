<template>
    <div class="container">
        <div>
            <b-card bg-variant="light" header="Konvertera fil" class="text-center">
                <b-card-text>
                    <b-row>
                        <b-col offset-md="1" md="3">
                            Ladda upp fil:
                        </b-col>
                        <b-col md="6">
                            <input type="file" id="file" ref="file" v-on:change="handleFileConvert()" />
                        </b-col>
                        <b-col md="2">
                            <b-button variant="secondary" size="sm" v-on:click="submitFile()">Konvertera</b-button>
                        </b-col>
                    </b-row>
                </b-card-text>

            </b-card>
        </div>
        <div class="result">
            <b-card bg-variant="light" header="Resultat">
                <section v-if="error">
                    <p>Ett fel intraffade under konverteringen.</p>
                    <p>{{errorMessage}}</p>
                </section>
                <section v-else>
                    <div v-if="loading">Loading...</div>
                    <div v-else>
                        {{convertedProfile}}
                    </div>

                </section>
            </b-card>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';

    export default {
        data() {
            return {
                file: '',
                convertedProfile: 'Ingen fil konverterad',
                error: false,
                errorMessage: '',
                loading: false
            }
        },
        methods: {
            submitFile() {
                let formData = new FormData();
                formData.append('file', this.file);
                this.loading = true;

                axios.post('/api/convert',
                    formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                }
                ).then(res => {
                    this.convertedProfile = res.data
                    this.error = ""
                })
                    .catch(error => {
                        this.error = true
                        this.errorMessage = error
                    })
                    .finally(() => this.loading = false)

            },
            handleFileConvert() {
                this.file = this.$refs.file.files[0];
            }
        }
    }
</script>

