import React, { useState } from 'react';
import { Dropdown } from 'react-bootstrap';
import './CustomDropdown.scss';
import Slider from 'rc-slider';// Custom styles for the range slider

function CustomDropdown() {
    const [priceRange, setPriceRange] = useState([20377, 1018850]);

    const handlePriceChange = (value) => {
        setPriceRange(value);
    };


    return (
        <Dropdown align="end" className="w-100 custom-dropdown">
            <Dropdown.Toggle variant="outline-secondary" id="dropdown-price" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                <div className="dropdown-text">
                    <span className="dropdown-tittle">Price per lesson: </span>
                    <span className="dropdown-choice">₫{priceRange[0].toLocaleString()} – ₫{priceRange[1].toLocaleString()}</span>
                </div>
            </Dropdown.Toggle>
            <Dropdown.Menu className="p-3" style={{ width: '300px' }}>
                <div>
                    <p className="text-center">
                        ₫{priceRange[0].toLocaleString()} – ₫{priceRange[1].toLocaleString()}+
                    </p>
                    <Slider
                        range
                        min={20377}
                        max={1018850}
                        defaultValue={priceRange}
                        onChange={handlePriceChange}
                    />
                </div>
            </Dropdown.Menu>
        </Dropdown>
    );
}

export default CustomDropdown;
