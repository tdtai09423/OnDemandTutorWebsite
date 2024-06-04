import React from 'react'
import './style.scss'
// import { Link } from 'react-router-dom'

function UserProfile() {

    return (
        <>
            <section className="h-100 gradient-custom-2">
                <div className="containerRoot py-5 h-100">
                    <div className="row d-flex justify-content-center containerRoot02">
                        <div className="col col-lg-9 col-xl-8">
                            <div className="card">
                                <div className="rounded-top text-white d-flex flex-row" style={{ backgroundColor: '#6c757d', height: '200px' }}>
                                    <div className="ms-4 mt-5 d-flex flex-column" style={{ width: '150px' }}>
                                        {/* avatar */}
                                        <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-profiles/avatar-1.webp"
                                            alt="Generic placeholder image" className="img-fluid img-thumbnail mt-4 mb-2"
                                            style={{ width: '150px', zIndex: 1 }} />
                                        <button type="button" data-mdb-button-init data-mdb-ripple-init className="editprofile" style={{ zIndex: 1 }}>
                                            Edit profile
                                        </button>
                                    </div>
                                    {/* first name, last name, nation      */}
                                    <div className="ms-3" style={{ marginTop: '130px' }}>
                                        <h5>Andy Horwitz</h5>
                                        <p>New York</p>
                                    </div>
                                </div>
                                <div className="p-4 text-black bg-body-tertiary">
                                    <div className="d-flex justify-content-end text-center py-1 text-body">
                                        <div>
                                            <p className="mb-1 h5">253</p>
                                            <p className="small text-muted mb-0">Photos</p>
                                        </div>

                                    </div>
                                </div>
                                <div className="card-body p-4 text-black">
                                    <div className="mb-5 text-body">
                                        <p className="lead fw-normal mb-1">About</p>
                                        <div className="p-4 bg-body-tertiary">
                                            <p className="font-italic mb-1">Web Developer</p>
                                            <p className="font-italic mb-1">Lives in New York</p>
                                            <p className="font-italic mb-0">Photographer</p>
                                        </div>
                                    </div>
                                    <div className="d-flex justify-content-between align-items-center mb-4 text-body">
                                        <p className="lead fw-normal mb-0">Recent photos</p>
                                        <p className="mb-0"><a href="#!" className="text-muted">Show all</a></p>
                                    </div>
                                    <div className="row g-2">
                                        <div className="col mb-2">
                                            <img src="https://mdbcdn.b-cdn.net/img/Photos/Lightbox/Original/img%20(112).webp" alt="image 1"
                                                className="w-100 rounded-3" />
                                        </div>
                                        <div className="col mb-2">
                                            <img src="https://mdbcdn.b-cdn.net/img/Photos/Lightbox/Original/img%20(107).webp" alt="image 1"
                                                className="w-100 rounded-3" />
                                        </div>
                                    </div>
                                    <div className="row g-2">
                                        <div className="col">
                                            <img src="https://mdbcdn.b-cdn.net/img/Photos/Lightbox/Original/img%20(108).webp" alt="image 1"
                                                className="w-100 rounded-3" />
                                        </div>
                                        <div className="col">
                                            <img src="https://mdbcdn.b-cdn.net/img/Photos/Lightbox/Original/img%20(114).webp" alt="image 1"
                                                className="w-100 rounded-3" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </>
    )
}

export default UserProfile