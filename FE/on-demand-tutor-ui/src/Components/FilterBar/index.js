import React from 'react';
import { Container, Row, Col, Form, InputGroup, Dropdown, Button } from 'react-bootstrap';
import 'rc-slider/assets/index.css';
import './FilterBar.scss';
import CustomDropdown from './components/CustomDropdown';

function FilterBar() {

    return (
        <Container fluid className="mt-4">
            <Row className="filter-bar-row">
                <Col md={3}>
                    <Dropdown align="end" className="w-100">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                            <div className="dropdown-text">
                                <span className="dropdown-tittle">I want to learn:</span>
                                <span className="dropdown-choice">English</span>
                            </div>
                        </Dropdown.Toggle>
                        <Dropdown.Menu className="dropdown-menu">
                            <Dropdown.Item href="#">English</Dropdown.Item>
                            <Dropdown.Item href="#">Spanish</Dropdown.Item>
                            <Dropdown.Item href="#">French</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
                <Col md={3}>
                    <CustomDropdown />
                </Col>
                <Col md={3}>
                    <Dropdown align="end" className="w-100">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                            <div className="dropdown-text">
                                <span className="dropdown-tittle">Country of birth:</span>
                                <span className="dropdown-choice">Any Country</span>
                            </div>
                        </Dropdown.Toggle>
                        <Dropdown.Menu className="dropdown-menu">
                            <Dropdown.Item href="#">English</Dropdown.Item>
                            <Dropdown.Item href="#">Spanish</Dropdown.Item>
                            <Dropdown.Item href="#">French</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
                <Col md={3}>
                    <Dropdown align="end" className="w-100">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                            <div className="dropdown-text">
                                <span className="dropdown-tittle">I'm available</span>
                                <span className="dropdown-choice">Anytime</span>
                            </div>
                        </Dropdown.Toggle>
                        <Dropdown.Menu className="dropdown-menu">
                            <Dropdown.Item href="#">English</Dropdown.Item>
                            <Dropdown.Item href="#">Spanish</Dropdown.Item>
                            <Dropdown.Item href="#">French</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
            </Row>
            <Row className="filter-bar-row">
                <Col md={2} className="d-flex justify-content-between">
                    <Dropdown align="end" className="w-100">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                            Also speaks
                        </Dropdown.Toggle>
                        <Dropdown.Menu className="dropdown-menu">
                            <Dropdown.Item href="#">Option 1</Dropdown.Item>
                            <Dropdown.Item href="#">Option 2</Dropdown.Item>
                            <Dropdown.Item href="#">Option 3</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
                <Col md={2} className="d-flex justify-content-between">
                    <Dropdown align="end" className="w-100">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                            Native speaker
                        </Dropdown.Toggle>
                        <Dropdown.Menu className="dropdown-menu">
                            <Dropdown.Item href="#">Yes</Dropdown.Item>
                            <Dropdown.Item href="#">No</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
                <Col md={2} className="d-flex justify-content-between">
                </Col>
                <Col md={2} className="d-flex justify-content-between">
                    <Dropdown align="end">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-sort-by" className="dropdown-toggle-multi-line rounded-2">
                            Sort by: Our top picks
                        </Dropdown.Toggle>
                        <Dropdown.Menu className="dropdown-menu">
                            <Dropdown.Item href="#">Option 1</Dropdown.Item>
                            <Dropdown.Item href="#">Option 2</Dropdown.Item>
                            <Dropdown.Item href="#">Option 3</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
                <Col md={3} className="d-flex justify-content-between">
                    <InputGroup className="">
                        <Form className="d-flex">
                            <Form.Control
                                type="search"
                                placeholder="Search"
                                className="me-2 search-bar"
                                aria-label="Search"
                            />

                        </Form>
                    </InputGroup>
                </Col>
                <Col md={1} className="d-flex justify-content-between">
                    <Button className="loginButton text-black border border-2 border-dark" variant="outline-success">
                        <span className="loginContent">Apply</span>
                    </Button>
                </Col>
            </Row>
        </Container>
    );
}

export default FilterBar;
