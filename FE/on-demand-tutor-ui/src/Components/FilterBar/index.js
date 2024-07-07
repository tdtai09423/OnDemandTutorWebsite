import React, { useEffect } from 'react';
import { Container, Row, Col, Form, InputGroup, Dropdown, Button } from 'react-bootstrap';
import 'rc-slider/assets/index.css';
import CustomDropdown from './components/CustomDropdown';
import { useState } from 'react';
import DatePicker from "react-widgets/DatePicker";
import './FilterBar.scss';


function FilterBar({ majors }) {

    const [major, setMajor] = useState('English');
    const [majorChoices, setMajorChoices] = useState([]);
    const [nationality, setNationality] = useState('Any Country');
    const [available, setAvailable] = useState('');
    const [native, setNative] = useState('No');
    const [also, setAlso] = useState('Also speak');
    const [sortBy, setSortBy] = useState('Our top picks');
    console.log(majors);

    const submitFilter = (e) => {
        console.log({ major, nationality, available, native, sortBy });
    }

    const handleSelectMajor = (major) => {
        setMajor(major); // update the major state when an option is selected
    };
    const handleSelectBirth = (birth) => {
        setNationality(birth); // update the nationality state when an option is selected
    };
    const handleDateChange = (date) => {
        setAvailable(date);
        console.log('Selected Date:', date); // You can process the selected date here
    };
    const handleSelectNative = (native) => {
        setNative(native); // update the nationality state when an option is selected
    };
    const handleSelectSortBy = (sortBy) => {
        setSortBy(sortBy); // update
    };

    useEffect(() => {
        setMajorChoices(majors);
    }, [])

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
                            {majorChoices.map((major) => (
                                <Dropdown.Item onClick={() => handleSelectMajor(`${major.majorName}`)}>
                                    {major.majorName}
                                </Dropdown.Item>
                            ))}
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
                    <DatePicker
                        selected={available}
                        onChange={handleDateChange}
                        minDate={new Date()}
                        id='custom-datepicker'
                        className="custom-datepicker" // Use a specific class for your custom styles
                        valueFormat={{ dateStyle: "medium" }}
                    />
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
                            Sort by: {sortBy}
                        </Dropdown.Toggle>
                        <Dropdown.Menu className="dropdown-menu">
                            <Dropdown.Item onClick={() => handleSelectSortBy('Ascendent by price')}>Ascendent by price</Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectSortBy('Descendant by price')}>Descendant by price</Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectSortBy('Ascendent by rating')}>Ascendent by rating</Dropdown.Item>
                            <Dropdown.Item onClick={() => handleSelectSortBy('Descendant by rating')}>Descendant by rating</Dropdown.Item>
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
                    <Button className="loginButton text-black border border-2 border-dark" variant="" onClick={submitFilter}>
                        <span className="loginContent">Apply</span>
                    </Button>
                </Col>
            </Row>
        </Container>
    );
}

export default FilterBar;
