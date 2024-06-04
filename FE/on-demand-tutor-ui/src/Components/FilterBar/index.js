import React from 'react';
import { Container, Row, Col, Form, InputGroup, Dropdown, Button } from 'react-bootstrap';
import 'rc-slider/assets/index.css';
import './FilterBar.scss';
import CustomDropdown from './components/CustomDropdown';
import { useState } from 'react';

function FilterBar() {

    const [major, setMajor] = useState('English');
    const [nationality, setNationality] = useState('Any Country');
    const [available, setAvailable] = useState('Anytime');
    const [native, setNative] = useState('No');
    const [also, setAlso] = useState('Also speak');

    const submitFilter = (e) => {
        console.log({ major, nationality, available, native });
    }

    const handleSelectMajor = (major) => {
        setMajor(major); // update the major state when an option is selected
    };
    const handleSelectBirth = (birth) => {
        setNationality(birth); // update the nationality state when an option is selected
    };
    const handleSelectAvailable = (available) => {
        setAvailable(available); // update the nationality state when an option is selected
    };
    const handleSelectNative = (native) => {
        setNative(native); // update the nationality state when an option is selected
    };

    return (
        <Container fluid className="mt-4">
            <Row className="filter-bar-row">
                <Col md={3}>
                    <Dropdown align="end" className="w-100">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                            <div className="dropdown-text">
                                <span className="dropdown-tittle">I want to learn:</span>
                                <span className="dropdown-choice">{major}</span>
                            </div>
                        </Dropdown.Toggle>
                        <Dropdown.Menu className=" dropdown-menu">
                            <Dropdown.Item onClick={() => handleSelectMajor('English')}>
                                English
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectMajor('Spanish')}>
                                Spanish
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectMajor('French')}>
                                French
                            </Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
                <Col md={3}>
                    <CustomDropdown
                        priceMin="0"
                        priceMax="1000000"
                    />
                </Col>
                <Col md={3}>
                    <Dropdown align="end" className="w-100">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                            <div className="dropdown-text">
                                <span className="dropdown-tittle">Country of birth:</span>
                                <span className="dropdown-choice">{nationality}</span>
                            </div>
                        </Dropdown.Toggle>
                        <Dropdown.Menu className=" dropdown-menu">
                            <Dropdown.Item onClick={() => handleSelectBirth('English')}>
                                English
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectBirth('Spanish')}>
                                Spanish
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectBirth('French')}>
                                French
                            </Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
                <Col md={3}>
                    <Dropdown align="end" className="w-100">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                            <div className="dropdown-text">
                                <span className="dropdown-tittle">I'm available</span>
                                <span className="dropdown-choice">{available}</span>
                            </div>
                        </Dropdown.Toggle>
                        <Dropdown.Menu className=" dropdown-menu">
                            <Dropdown.Item onClick={() => handleSelectAvailable('Anytime')}>
                                Anytime
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectAvailable('Monday')}>
                                Monday
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectAvailable('Tuesday')}>
                                Tuesday
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectAvailable('Wednesday')}>
                                Wednesday
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectAvailable('Thursday')}>
                                Thursday
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectAvailable('Friday')}>
                                Friday
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectAvailable('Saturday')}>
                                Saturday
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectAvailable('Sunday')}>
                                Sunday
                            </Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
            </Row>
            <Row className="filter-bar-row">
                <Col md={2} className="d-flex justify-content-between">
                    <Dropdown align="end" className="w-100">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                            {also}
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
                            Native speak: {native}
                        </Dropdown.Toggle>
                        <Dropdown.Menu className=" dropdown-menu">
                            <Dropdown.Item onClick={() => handleSelectNative('Yes')}>
                                Yes
                            </Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectNative('No')}>
                                No
                            </Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Col>
                <Col md={2} className="d-flex justify-content-between">
                </Col>
                <Col md={2} className="d-flex justify-content-between">
                    <Dropdown align="end">
                        <Dropdown.Toggle variant="outline-secondary" id="dropdown-sort-by" className="w-100 dropdown-toggle-multi-line rounded-2">
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
                    <Button className="loginButton text-black border border-2 border-dark" variant="outline-success" onClick={submitFilter}>
                        <span className="loginContent">Apply</span>
                    </Button>
                </Col>
            </Row>
        </Container>
    );
}

export default FilterBar;
